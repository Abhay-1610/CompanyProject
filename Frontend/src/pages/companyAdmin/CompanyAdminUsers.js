import React, { useEffect, useState } from "react";
import { useDispatch, useSelector } from "react-redux";

/* ================= USERS REDUX ================= */
import {
  fetchUsersByCompany,
  createUser,
  updateUser,
  deleteUser,
  toggleBlockUser
} from "../../features/users/usersSlice";

/* ================= FORMIK + YUP ================= */
import { Formik } from "formik";
import * as Yup from "yup";

/* ================= TOAST ================= */
import { toast } from "react-toastify";
import Swal from "sweetalert2";

function CompanyAdminUsers() {
  /* ================= AUTH CONTEXT ================= */
  const { companyId, userId } = useSelector((state) => state.auth);

  /* ================= REDUX USERS STATE ================= */
  const dispatch = useDispatch();
  const { list: users, loading } = useSelector((state) => state.users);

  /* ================= UI STATE ================= */
  const [showModal, setShowModal] = useState(false);
  const [mode, setMode] = useState("create");
  const [selectedUserId, setSelectedUserId] = useState(null);

  /* ================= FETCH USERS ================= */
  useEffect(() => {
    if (companyId) {
      dispatch(fetchUsersByCompany(companyId));
    }
  }, [companyId, dispatch]);

  /* ================= FILTER SELF ================= */
  const visibleUsers = users.filter(
    (u) => u.id !== userId
  );

  /* ================= FORM VALIDATION ================= */
  const validationSchema = Yup.object({
    email: Yup.string()
      .email("Invalid email")
      .required("Email is required"),
    password: Yup.string().when("mode", {
      is: "create",
      then: (schema) =>
        schema.required("Password is required"),
      otherwise: (schema) => schema.notRequired()
    })
  });

  /* ================= SAVE USER ================= */
  const handleSave = async (values, { setSubmitting, resetForm }) => {
  try {
    if (mode === "create") {
      await dispatch(
        createUser({
          email: values.email,
          password: values.password,
          companyId,
          role: "CompanyUser"
        })
      ).unwrap(); // 🔥 REQUIRED

      await dispatch(fetchUsersByCompany(companyId)).unwrap();

      toast.success("User created");
    } else {
      await dispatch(
        updateUser({
          userId: selectedUserId,
          email: values.email
        })
      ).unwrap();

      toast.success("User updated");
    }

    resetForm();
    setSelectedUserId(null);
    setShowModal(false);
  } catch (err) {
  } finally {
    setSubmitting(false);
  }
};


   /* ================= Delete USER ================= */

   const handleDelete = async (id) => {
    const result = await Swal.fire({
      title: "Delete User?",
      text: "This action cannot be undone",
      icon: "warning",
      showCancelButton: true,
      confirmButtonColor: "#d33",
      confirmButtonText: "Yes, delete",
      cancelButtonText: "Cancel",
    });
  
    if (!result.isConfirmed) return;
    // await Swal.fire("Deleted!", "User deleted", "success");
  
    try {
      await dispatch(deleteUser(id)).unwrap();
      toast.success("User deleted");
    } catch {
      // delete blocked → handled globally
    }
  };

  return (
    <>
      {/* ================= PAGE ================= */}
      <div
        className="min-vh-100"
        style={{
          backgroundColor: "#FBF7F2",
          padding: "40px"
        }}
      >
        {/* Header */}
        <div className="d-flex justify-content-between align-items-center mb-4">
          <div>
            <h3 className="fw-bold text-dark mb-1">
              Company Users
            </h3>
            <p className="text-muted mb-0">
              Manage users in your company
            </p>
          </div>

          <button
            className="btn btn-dark"
            onClick={() => {
              setMode("create");
              setSelectedUserId(null);
              setShowModal(true);
            }}
          >
            + Add User
          </button>
        </div>

        {/* Users Table */}
        <div className="bg-white border rounded p-4">
          {loading && (
            <p className="text-muted small">Loading users...</p>
          )}

          <table className="table align-middle mb-0">
            <thead>
              <tr className="text-muted small">
                <th>Email</th>
                <th>Role</th>
                <th>Status</th>
                <th className="text-end">Actions</th>
              </tr>
            </thead>

            <tbody>
              {visibleUsers.length === 0 && !loading ? (
                <tr>
                  <td colSpan="4" className="text-center text-muted py-4">
                    No users found
                  </td>
                </tr>
              ) : (
                visibleUsers.map((u) => (
                  <tr key={u.id}>
                    <td className="fw-semibold text-dark">
                      {u.email}
                    </td>
                    <td>{u.role}</td>
                    <td>
                      <span
                        className={`badge ${
                          u.isBlocked ? "bg-danger" : "bg-success"
                        }`}
                      >
                        {u.isBlocked ? "Blocked" : "Active"}
                      </span>
                    </td>
                    <td className="text-end">
                      <button
                        className="btn btn-sm btn-outline-dark me-2"
                        onClick={() => {
                          setMode("edit");
                          setSelectedUserId(u.id);
                          setShowModal(true);
                        }}
                      >
                        Edit
                      </button>

                      <button
                        className={`btn btn-sm me-2 ${
                          u.isBlocked
                            ? "btn-outline-success"
                            : "btn-outline-warning"
                        }`}
                        onClick={() =>
                          dispatch(toggleBlockUser(u.id))
                        }
                      >
                        {u.isBlocked ? "Unblock" : "Block"}
                      </button>

                      <button
                        className="btn btn-sm btn-outline-danger"
                        onClick={() =>
                          handleDelete(u.id)
                        }
                      >
                        Delete
                      </button>
                    </td>
                  </tr>
                ))
              )}
            </tbody>
          </table>
        </div>
      </div>

      {/* ================= MODAL (FORMIK) ================= */}
      {showModal && (
        <Formik
          enableReinitialize
          initialValues={{
            email:
              mode === "edit"
                ? users.find(u => u.id === selectedUserId)?.email || ""
                : "",
            password: "",
            mode
          }}
          validationSchema={validationSchema}
          onSubmit={handleSave}
        >
          {({
            values,
            errors,
            touched,
            handleChange,
            handleSubmit,
            isSubmitting
          }) => (
            <form onSubmit={handleSubmit}>
              <div
                className="modal fade show d-block"
                style={{ backgroundColor: "rgba(0,0,0,0.5)" }}
              >
                <div className="modal-dialog modal-dialog-centered">
                  <div className="modal-content">
                    <div className="modal-header">
                      <h5 className="modal-title fw-semibold">
                        {mode === "create" ? "Add User" : "Edit User"}
                      </h5>
                      <button
                        type="button"
                        className="btn-close"
                        onClick={() => setShowModal(false)}
                      />
                    </div>

                    <div className="modal-body">
                      {/* EMAIL */}
                      <div className="mb-3">
                        <label className="form-label">Email</label>
                        <input
                          name="email"
                          className="form-control"
                          value={values.email}
                          onChange={handleChange}
                        />
                        {touched.email && errors.email && (
                          <small className="text-danger">{errors.email}</small>
                        )}
                      </div>

                      {/* PASSWORD (CREATE ONLY) */}
                      {mode === "create" && (
                        <div className="mb-3">
                          <label className="form-label">Password</label>
                          <input
                            type="password"
                            name="password"
                            className="form-control"
                            value={values.password}
                            onChange={handleChange}
                          />
                          {touched.password && errors.password && (
                            <small className="text-danger">
                              {errors.password}
                            </small>
                          )}
                        </div>
                      )}
                    </div>

                    <div className="modal-footer">
                      <button
                        type="button"
                        className="btn btn-outline-dark"
                        onClick={() => setShowModal(false)}
                      >
                        Cancel
                      </button>
                      <button
                        type="submit"
                        className="btn btn-dark"
                        disabled={isSubmitting}
                      >
                        Save
                      </button>
                    </div>
                  </div>
                </div>
              </div>
            </form>
          )}
        </Formik>
      )}
    </>
  );
}

export default CompanyAdminUsers;

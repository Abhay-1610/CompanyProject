import React, { useEffect, useState } from "react";
import { useSearchParams } from "react-router-dom";
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

function SuperAdminUsers() {
  const [searchParams] = useSearchParams();
  const companyId = searchParams.get("companyId");

  const dispatch = useDispatch();

  /* ================= REDUX STATE ================= */
  const { list: users, loading } = useSelector(
    (state) => state.users
  );

  /* ================= UI STATE ================= */
  const [showModal, setShowModal] = useState(false);
  const [mode, setMode] = useState("create"); // create | edit
  const [selectedUserId, setSelectedUserId] = useState(null);

  /* ================= FETCH USERS ================= */
  useEffect(() => {
    if (companyId) {
      dispatch(fetchUsersByCompany(companyId));
    }
  }, [companyId, dispatch]);

  /* ================= FORM VALIDATION ================= */
  const validationSchema = Yup.object({
    email: Yup.string()
      .email("Invalid email")
      .required("Email is required"),
    password: Yup.string().when("mode", {
      is: "create",
      then: (schema) => schema.required("Password is required"),
      otherwise: (schema) => schema.notRequired()
    }),
    role: Yup.string().required("Role is required")
  });

  /* ================= SAVE USER ================= */
  const handleSave = async (values, { setSubmitting, resetForm }) => {
    try {
      if (mode === "create") {
        await dispatch(
          createUser({
            email: values.email,
            password: values.password,
            role: values.role,
            companyId: Number(companyId)
          })
        ).unwrap();
        toast.success("User created");
      } else {
        await dispatch(
          updateUser({
            userId: selectedUserId,
            email: values.email,
            role: values.role
          })
        ).unwrap();
        toast.success("User updated");
      }

      resetForm();
      setSelectedUserId(null);
      setMode("create");
      setShowModal(false);
    } catch {
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

  /* ================= UI ================= */
  return (
    <>
      <div
        className="min-vh-100"
        style={{ backgroundColor: "#FBF7F2", padding: "40px" }}
      >
        {/* ================= HEADER ================= */}
        <div className="d-flex justify-content-between align-items-center mb-4">
          <h4>Company Users</h4>

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

        {/* ================= USERS TABLE ================= */}
        <div className="bg-white border rounded p-4">
          {loading && <p>Loading users...</p>}

          <table className="table align-middle">
            <thead>
              <tr>
                <th>Email</th>
                <th>Role</th>
                <th>Status</th>
                <th className="text-end">Actions</th>
              </tr>
            </thead>

            <tbody>
              {users.map((u) => (
                <tr key={u.id}>
                  <td>{u.email}</td>
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
                      className="btn btn-sm btn-outline-warning me-2"
                      onClick={() => dispatch(toggleBlockUser(u.id))}
                    >
                      {u.isBlocked ? "Unblock" : "Block"}
                    </button>

                    <button
                      className="btn btn-sm btn-outline-danger"
                      onClick={() => handleDelete(u.id)}
                    >
                      Delete
                    </button>
                  </td>
                </tr>
              ))}

              {users.length === 0 && !loading && (
                <tr>
                  <td colSpan="4" className="text-center text-muted py-4">
                    No users found
                  </td>
                </tr>
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
            role:
              mode === "edit"
                ? users.find(u => u.id === selectedUserId)?.role || "CompanyUser"
                : "CompanyUser",
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
                style={{ background: "rgba(0,0,0,0.5)" }}
              >
                <div className="modal-dialog modal-dialog-centered">
                  <div className="modal-content">
                    <div className="modal-header">
                      <h5>
                        {mode === "create" ? "Add User" : "Edit User"}
                      </h5>
                      <button
                        type="button"
                        className="btn-close"
                        onClick={() => setShowModal(false)}
                      />
                    </div>

                    <div className="modal-body">
                      {/* Email */}
                      <input
                        name="email"
                        className="form-control mb-2"
                        placeholder="Email"
                        value={values.email}
                        onChange={handleChange}
                      />
                      {touched.email && errors.email && (
                        <small className="text-danger">
                          {errors.email}
                        </small>
                      )}

                      {/* Password (Create only) */}
                      {mode === "create" && (
                        <>
                          <input
                            type="password"
                            name="password"
                            className="form-control mb-2"
                            placeholder="Password"
                            value={values.password}
                            onChange={handleChange}
                          />
                          {touched.password && errors.password && (
                            <small className="text-danger">
                              {errors.password}
                            </small>
                          )}
                        </>
                      )}

                      {/* Role */}
                      <select
                        name="role"
                        className="form-select"
                        value={values.role}
                        onChange={handleChange}
                      >
                        <option value="CompanyAdmin">Company Admin</option>
                        <option value="CompanyUser">Company User</option>
                      </select>
                      {touched.role && errors.role && (
                        <small className="text-danger">
                          {errors.role}
                        </small>
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

export default SuperAdminUsers;

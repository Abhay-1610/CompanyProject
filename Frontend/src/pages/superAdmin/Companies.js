import React, { useEffect, useState } from "react";
import { useDispatch, useSelector } from "react-redux";

/* ================= REDUX ================= */
import {
  fetchCompanies,
  createCompany,
  updateCompany,
  deleteCompany
} from "../../features/companies/companiesSlice";

/* ================= FORMIK + YUP ================= */
import { Formik } from "formik";
import * as Yup from "yup";

/* ================= TOAST ================= */
import { toast } from "react-toastify";
import Swal from "sweetalert2";

function SuperAdminCompanies() {
  /* ================= REDUX STATE ================= */
  const dispatch = useDispatch();
  const { list: companies, loading } = useSelector(
    (state) => state.companies
  );

  /* ================= UI STATE ================= */
  const [showModal, setShowModal] = useState(false);
  const [mode, setMode] = useState("create"); // create | edit
  const [selectedId, setSelectedId] = useState(null);

  /* ================= FETCH COMPANIES ================= */
  useEffect(() => {
    dispatch(fetchCompanies());
  }, [dispatch]);

  /* ================= FORM VALIDATION ================= */
  const validationSchema = Yup.object({
    companyName: Yup.string()
      .required("Company name is required")
  });

  /* ================= SAVE (CREATE / UPDATE) ================= */
  const handleSave = async (values, { setSubmitting, resetForm }) => {
  try {
    if (mode === "create") {
      await dispatch(
        createCompany({ companyName: values.companyName })
      ).unwrap();

      toast.success("Company created");
    } else {
      await dispatch(
        updateCompany({
          id: selectedId,
          companyName: values.companyName
        })
      ).unwrap();

      toast.success("Company updated");
    }

    resetForm();
    setSelectedId(null);
    setMode("create");
    setShowModal(false);
  } catch (error) {
  } finally {
    setSubmitting(false);
  }
};


  /* ================= DELETE ================= */
  const handleDelete = async (id) => {
    const result = await Swal.fire({
          title: "Delete Company?",
          text: "This action cannot be undone",
          icon: "warning",
          showCancelButton: true,
          confirmButtonColor: "#d33",
          confirmButtonText: "Yes, delete",
          cancelButtonText: "Cancel",
        });
      
    if (!result.isConfirmed) return;
    
  try {
    await dispatch(deleteCompany(id)).unwrap();
    toast.success("Company deleted");
  } catch (err) {
    
  }
};

  return (
    <>
      {/* ================= PAGE ================= */}
      <div
        className="min-vh-100"
        style={{ backgroundColor: "#FBF7F2", padding: "40px" }}
      >
        {/* Header */}
        <div className="d-flex justify-content-between align-items-center mb-4">
          <div>
            <h3 className="fw-bold text-dark mb-1">Companies</h3>
            <p className="text-muted mb-0">
              Manage registered companies
            </p>
          </div>

          <button
            className="btn btn-dark"
            onClick={() => {
              setMode("create");
              setSelectedId(null);
              setShowModal(true);
            }}
          >
            + Add Company
          </button>
        </div>

        {/* Table */}
        <div className="bg-white border rounded p-4">
          {loading && (
            <p className="text-muted small mb-2">
              Loading companies...
            </p>
          )}

          <table className="table align-middle mb-0">
            <thead>
              <tr className="text-muted small">
                <th>Company Name</th>
                <th>Status</th>
                <th>Created On</th>
                <th className="text-end">Actions</th>
              </tr>
            </thead>

            <tbody>
              {companies.length === 0 && !loading ? (
                <tr>
                  <td colSpan="4" className="text-center text-muted py-4">
                    No companies found
                  </td>
                </tr>
              ) : (
                companies.map((c) => (
                  <tr key={c.companyId}>
                    <td className="fw-semibold text-dark">
                      {c.companyName}
                    </td>

                    <td>
                      <span
                        className={`badge ${
                          c.isActive ? "bg-success" : "bg-secondary"
                        }`}
                      >
                        {c.isActive ? "Active" : "Inactive"}
                      </span>
                    </td>

                    <td className="text-muted">
                      {new Date(c.createdAt).toLocaleDateString()}
                    </td>

                    <td className="text-end">
                      <button
                        className="btn btn-sm btn-outline-dark me-2"
                        onClick={() => {
                          setMode("edit");
                          setSelectedId(c.companyId);
                          setShowModal(true);
                        }}
                      >
                        Edit
                      </button>

                      <button
                        className="btn btn-sm btn-outline-danger"
                        onClick={() => handleDelete(c.companyId)}
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
            companyName:
              mode === "edit"
                ? companies.find(c => c.companyId === selectedId)?.companyName || ""
                : ""
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
                        {mode === "create" ? "Add Company" : "Edit Company"}
                      </h5>
                      <button
                        type="button"
                        className="btn-close"
                        onClick={() => setShowModal(false)}
                      />
                    </div>

                    <div className="modal-body">
                      <label className="form-label">
                        Company Name
                      </label>
                      <input
                        name="companyName"
                        type="text"
                        className="form-control"
                        value={values.companyName}
                        onChange={handleChange}
                      />
                      {touched.companyName && errors.companyName && (
                        <small className="text-danger">
                          {errors.companyName}
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

export default SuperAdminCompanies;

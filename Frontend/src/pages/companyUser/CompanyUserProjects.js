import React, { useEffect, useState } from "react";
import { useDispatch, useSelector } from "react-redux";

/* ================= PROJECTS REDUX ================= */
import {
  fetchProjectsByCompany,
  createProject,
  updateProject,
  deleteProject
} from "../../features/projects/projectsSlice";

/* ================= FORMIK + YUP ================= */
import { Formik } from "formik";
import * as Yup from "yup";

/* ================= TOAST ================= */
import { toast } from "react-toastify";
import Swal from "sweetalert2";

function CompanyUserProjects() {
  const dispatch = useDispatch();

  /* ================= AUTH ================= */
  const companyId = useSelector((state) => state.auth.companyId);

  /* ================= REDUX ================= */
  const { list: projects, loading } = useSelector(
    (state) => state.projects
  );

  /* ================= UI STATE ================= */
  const [showModal, setShowModal] = useState(false);
  const [mode, setMode] = useState("create"); // create | edit
  const [selectedProjectId, setSelectedProjectId] = useState(null);

  /* ================= FETCH PROJECTS ================= */
  useEffect(() => {
    if (companyId) {
      dispatch(fetchProjectsByCompany(companyId));
    }
  }, [dispatch, companyId]);

  /* ================= FORM VALIDATION ================= */
  const validationSchema = Yup.object({
    projectName: Yup.string().required("Project name is required"),
    description: Yup.string().required("Description is required"),
    startDate: Yup.string().required("Start date is required"),
    endDate: Yup.string().required("End date is required")
  });

  /* ================= SAVE HANDLER ================= */
  const handleSave = async (values, { setSubmitting, resetForm }) => {
    try {
      const payload = {
        projectName: values.projectName,
        description: values.description,
        startDate: new Date(values.startDate).toISOString(),
        endDate: new Date(values.endDate).toISOString(),
        companyId
      };

      if (mode === "create") {
        
        setShowModal(false);
        await dispatch(createProject(payload)).unwrap();
        toast.success("Project created");
      } else {
        
      setShowModal(false);
        await dispatch(          
          updateProject({
            projectId: selectedProjectId,
            data: payload
          })
        ).unwrap();
        toast.success("Project updated");
      }

      resetForm();
      setSelectedProjectId(null);
    } catch {
      // error already handled globally (interceptor / alert)
    } finally {
      setSubmitting(false);
    }
  };
 
  /* ================= DELETE HANDLER ================= */
  const handleDelete = async (projectId) => {
    const result = await Swal.fire({
          title: "Delete Project?",
          text: "This action cannot be undone",
          icon: "warning",
          showCancelButton: true,
          confirmButtonColor: "#d33",
          confirmButtonText: "Yes, delete",
          cancelButtonText: "Cancel",
        });
      
    if (!result.isConfirmed) return;
    
    try {
      await dispatch(deleteProject(projectId)).unwrap();
      toast.success("Project deleted");
    } catch {
      // delete blocked → handled globally, do nothing here
    }
  };

  return (
    <>
      {/* ================= PAGE ================= */}
      <div
        className="min-vh-100"
        style={{ backgroundColor: "#FBF7F2", padding: 40 }}
      >
        {/* ================= HEADER ================= */}
        <div className="d-flex justify-content-between mb-4">
          <div>
            <h3 className="fw-bold text-dark mb-1">Projects</h3>
            <p className="text-muted mb-0">
              Manage your company projects
            </p>
          </div>

          <button
            className="btn btn-dark"
            onClick={() => {
              setMode("create");
              setSelectedProjectId(null);
              setShowModal(true);
            }}
          >
            + Add Project
          </button>
        </div>

        {/* ================= TABLE ================= */}
        <div className="bg-white border rounded p-4">
          {loading && (
            <p className="text-muted small">
              Loading projects...
            </p>
          )}

          <table className="table align-middle mb-0">
            <thead>
              <tr className="text-muted small">
                <th>Name</th>
                <th>Start</th>
                <th>End</th>
                <th>Status</th>
                <th className="text-end">Actions</th>
              </tr>
            </thead>

            <tbody>
              {!loading && projects.length === 0 ? (
                <tr>
                  <td colSpan="5" className="text-center text-muted py-4">
                    No projects found
                  </td>
                </tr>
              ) : (
                projects.map((p) => (
                  <tr key={p.projectId}>
                    <td className="fw-semibold text-dark">
                      {p.projectName}
                    </td>

                    <td className="text-muted">
                      {new Date(p.startDate).toLocaleDateString()}
                    </td>

                    <td className="text-muted">
                      {new Date(p.endDate).toLocaleDateString()}
                    </td>

                    <td>
                      <span
                        className={`badge ${
                          p.status === "InProgress"
                            ? "bg-success"
                            : "bg-secondary"
                        }`}
                      >
                        {p.status}
                      </span>
                    </td>

                    <td className="text-end">
                      <button
                        className="btn btn-sm btn-outline-dark me-2"
                        onClick={() => {
                          setMode("edit");
                          setSelectedProjectId(p.projectId);
                          setShowModal(true);
                        }}
                      >
                        Edit
                      </button>

                      <button
                        className="btn btn-sm btn-outline-danger"
                        onClick={() => handleDelete(p.projectId)}
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
            projectName:
              mode === "edit"
                ? projects.find(p => p.projectId === selectedProjectId)?.projectName || ""
                : "",
            description:
              mode === "edit"
                ? projects.find(p => p.projectId === selectedProjectId)?.description || ""
                : "",
            startDate:
              mode === "edit"
                ? projects.find(p => p.projectId === selectedProjectId)?.startDate?.slice(0, 10) || ""
                : "",
            endDate:
              mode === "edit"
                ? projects.find(p => p.projectId === selectedProjectId)?.endDate?.slice(0, 10) || ""
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

                    {/* Header */}
                    <div className="modal-header">
                      <h5 className="modal-title fw-semibold">
                        {mode === "create" ? "Add Project" : "Edit Project"}
                      </h5>
                      <button
                        type="button"
                        className="btn-close"
                        onClick={() => setShowModal(false)}
                      />
                    </div>

                    {/* Body */}
                    <div className="modal-body">
                      <input
                        name="projectName"
                        className="form-control mb-2"
                        placeholder="Project Name"
                        value={values.projectName}
                        onChange={handleChange}
                      />
                      {touched.projectName && errors.projectName && (
                        <small className="text-danger">{errors.projectName}</small>
                      )}

                      <textarea
                        name="description"
                        className="form-control mb-2"
                        placeholder="Description"
                        value={values.description}
                        onChange={handleChange}
                      />
                      {touched.description && errors.description && (
                        <small className="text-danger">{errors.description}</small>
                      )}

                      <input
                        type="date"
                        name="startDate"
                        className="form-control mb-2"
                        value={values.startDate}
                        onChange={handleChange}
                      />
                      {touched.startDate && errors.startDate && (
                        <small className="text-danger">{errors.startDate}</small>
                      )}

                      <input
                        type="date"
                        name="endDate"
                        className="form-control"
                        value={values.endDate}
                        onChange={handleChange}
                      />
                      {touched.endDate && errors.endDate && (
                        <small className="text-danger">{errors.endDate}</small>
                      )}
                    </div>

                    {/* Footer */}
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

export default CompanyUserProjects;

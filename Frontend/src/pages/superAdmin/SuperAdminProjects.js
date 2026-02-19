import React, { useEffect, useState } from "react";
import { useSearchParams } from "react-router-dom";
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

function SuperAdminProjects() {
  const [searchParams] = useSearchParams();
  const companyId = searchParams.get("companyId");
  const dispatch = useDispatch();

  /* ================= REDUX STATE ================= */
  const { list: projects, loading } = useSelector(
    (state) => state.projects
  );

  /* ================= UI STATE ================= */
  const [showModal, setShowModal] = useState(false);
  const [mode, setMode] = useState("create"); // create | edit
  const [selectedId, setSelectedId] = useState(null);

  /* ================= FETCH PROJECTS ================= */
  useEffect(() => {
    if (companyId) {
      dispatch(fetchProjectsByCompany(companyId));
    }
  }, [companyId, dispatch]);

  /* ================= FORM VALIDATION ================= */
  const validationSchema = Yup.object({
    projectName: Yup.string().required("Project name is required"),
    description: Yup.string().required("Description is required"),
    startDate: Yup.string().required("Start date is required"),
    endDate: Yup.string().required("End date is required")
  });

  /* ================= SAVE ================= */
  const handleSave = async (values, { setSubmitting, resetForm }) => {
  try {
    if (mode === "create") {
      await dispatch(
        createProject({
          projectName: values.projectName,
          description: values.description,
          startDate: values.startDate,
          endDate: values.endDate,
          companyId: Number(companyId)
        })
      ).unwrap();

      toast.success("Project created");
    } else {
      await dispatch(
        updateProject({
          projectId: selectedId,
          data: {
            projectName: values.projectName,
            description: values.description,
            startDate: values.startDate,
            endDate: values.endDate
          }
        })
      ).unwrap();

      toast.success("Project updated");
    }

    resetForm();
    setSelectedId(null);
    setMode("create");
    setShowModal(false);
  } catch (err) {
  } finally {
    setSubmitting(false);
  }
};


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
    toast.error("Failed to delete project");
  }
};

  return (
    <div
      className="min-vh-100 p-5"
      style={{ backgroundColor: "#FBF7F2" }}
    >
      {/* ================= HEADER ================= */}
      <div className="d-flex justify-content-between mb-3">
        <h4>Projects</h4>
        <button
          className="btn btn-dark"
          onClick={() => {
            setMode("create");
            setSelectedId(null);
            setShowModal(true);
          }}
        >
          + Add Project
        </button>
      </div>

      {/* ================= TABLE ================= */}
      <div className="bg-white border rounded p-4">
        {loading && (
          <p className="text-muted small mb-2">
            Loading projects...
          </p>
        )}

        <table className="table align-middle mb-0">
          <thead>
            <tr className="text-muted small">
              <th>Name</th>
              <th>Start</th>
              <th>End</th>
              <th className="text-end">Actions</th>
            </tr>
          </thead>

          <tbody>
            {projects.length === 0 && !loading ? (
              <tr>
                <td colSpan="4" className="text-center text-muted py-4">
                  No projects found
                </td>
              </tr>
            ) : (
              projects.map((p) => (
                <tr key={p.projectId}>
                  <td>{p.projectName}</td>
                  <td>{new Date(p.startDate).toLocaleDateString()}</td>
                  <td>{new Date(p.endDate).toLocaleDateString()}</td>
                  <td className="text-end">
                    <button
                      className="btn btn-sm btn-outline-dark me-2"
                      onClick={() => {
                        setMode("edit");
                        setSelectedId(p.projectId);
                        setShowModal(true);
                      }}
                    >
                      Edit
                    </button>

                    <button
                      className="btn btn-sm btn-outline-danger"
                      onClick={() =>
                        handleDelete(p.projectId)
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

      {/* ================= MODAL (FORMIK) ================= */}
      {showModal && (
        <Formik
          enableReinitialize
          initialValues={{
            projectName:
              mode === "edit"
                ? projects.find(p => p.projectId === selectedId)?.projectName || ""
                : "",
            description:
              mode === "edit"
                ? projects.find(p => p.projectId === selectedId)?.description || ""
                : "",
            startDate:
              mode === "edit"
                ? projects.find(p => p.projectId === selectedId)?.startDate?.slice(0, 10) || ""
                : "",
            endDate:
              mode === "edit"
                ? projects.find(p => p.projectId === selectedId)?.endDate?.slice(0, 10) || ""
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
                style={{ background: "rgba(0,0,0,0.5)" }}
              >
                <div className="modal-dialog modal-dialog-centered">
                  <div className="modal-content">
                    <div className="modal-header">
                      <h5>
                        {mode === "create" ? "Add Project" : "Edit Project"}
                      </h5>
                      <button
                        type="button"
                        className="btn-close"
                        onClick={() => setShowModal(false)}
                      />
                    </div>

                    <div className="modal-body">
                      {/* Project Name */}
                      <input
                        name="projectName"
                        className="form-control mb-2"
                        placeholder="Project Name"
                        value={values.projectName}
                        onChange={handleChange}
                      />
                      {touched.projectName && errors.projectName && (
                        <small className="text-danger">
                          {errors.projectName}
                        </small>
                      )}

                      {/* Description */}
                      <textarea
                        name="description"
                        className="form-control mb-2"
                        placeholder="Description"
                        value={values.description}
                        onChange={handleChange}
                      />
                      {touched.description && errors.description && (
                        <small className="text-danger">
                          {errors.description}
                        </small>
                      )}

                      {/* Start Date */}
                      <input
                        type="date"
                        name="startDate"
                        className="form-control mb-2"
                        value={values.startDate}
                        onChange={handleChange}
                      />
                      {touched.startDate && errors.startDate && (
                        <small className="text-danger">
                          {errors.startDate}
                        </small>
                      )}

                      {/* End Date */}
                      <input
                        type="date"
                        name="endDate"
                        className="form-control"
                        value={values.endDate}
                        onChange={handleChange}
                      />
                      {touched.endDate && errors.endDate && (
                        <small className="text-danger">
                          {errors.endDate}
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
    </div>
  );
}

export default SuperAdminProjects;

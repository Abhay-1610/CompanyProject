import React from "react";
import { Link } from "react-router-dom";
import { useSelector } from "react-redux";

function CompanyAdminDashboard() {
  /* =============================
     AUTH CONTEXT
  ============================== */
  const { companyName } = useSelector((state) => state.auth);

  return (
    <div
      className="min-vh-100"
      style={{
        backgroundColor: "#FBF7F2",
        padding: "40px"
      }}
    >
      {/* Header */}
      <div className="mb-4">
        <h3 className="fw-bold text-dark mb-1">
          Company Admin Dashboard
        </h3>
        <p className="text-muted mb-0">
          Manage users, projects, and company activity
        </p>
      </div>

      {/* ================= COMPANY INFO ================= */}
      <div className="mb-5">
        <div className="bg-white border rounded p-4">
          <h6 className="fw-semibold text-dark mb-2">
            Company Information
          </h6>
          <p className="text-muted mb-1">
            Company Name: <strong>{companyName}</strong>
          </p>
          <p className="text-muted mb-0">
            Status: <span className="badge bg-success">Active</span>
          </p>
        </div>
      </div>

      {/* ================= ACTION CARDS ================= */}
      <div className="row g-4">

        <div className="col-md-4">
          <div className="border rounded p-4 h-100 bg-white">
            <h5 className="fw-semibold text-dark mb-2">
              Company Users
            </h5>
            <p className="text-muted small mb-4">
              Create, update, block, or remove users in your company.
            </p>
            <Link to="/company-admin/users" className="btn btn-dark w-100">
              Manage Users
            </Link>
          </div>
        </div>

        <div className="col-md-4">
          <div className="border rounded p-4 h-100 bg-white">
            <h5 className="fw-semibold text-dark mb-2">
              Projects
            </h5>
            <p className="text-muted small mb-4">
              Create, update, and manage projects for your company.
            </p>
            <Link to="/company-admin/project" className="btn btn-dark w-100">
              Manage Projects
            </Link>
          </div>
        </div>

        <div className="col-md-4">
          <div className="border rounded p-4 h-100 bg-white">
            <h5 className="fw-semibold text-dark mb-2">
              Change History
            </h5>
            <p className="text-muted small mb-4">
              Track all user and project actions within your company.
            </p>
            <Link to="/company-admin/change-history" className="btn btn-dark w-100">
              View Audit Log
            </Link>
          </div>
        </div>

      </div>
    </div>
  );
}

export default CompanyAdminDashboard;

import { Link } from "react-router-dom";
import { useSelector } from "react-redux";
import React, { useEffect, useRef } from "react";


function CompanyUserDashboard() {

  /* =============================
     AUTH CONTEXT
  ============================== */
  const { companyName } = useSelector((state) => state.auth);

  /* =============================
     SIGNALR LIVE STATE
  ============================== */
  const audits = useSelector((state) => state.signalR.audits);
  const onlineCount = useSelector((state) => state.signalR.onlineCount);


  const activityEndRef = useRef(null);
  useEffect(() => {
  if (activityEndRef.current) {
    activityEndRef.current.scrollIntoView({ behavior: "smooth" });
  }
  }, [audits]);
  
  
  return (
    <div
      className="min-vh-100"
      style={{ backgroundColor: "#FBF7F2", padding: 40 }}
    >
      {/* Header */}
      <div className="mb-4">
        <h3 className="fw-bold text-dark mb-1">
          User Dashboard
        </h3>
        <p className="text-muted mb-0">
          Manage your projects and track company activity
        </p>
      </div>

      {/* ================= COMPANY INFO ================= */}
      <div className="mb-5">
        <div className="bg-white border rounded p-4">
          <h6 className="fw-semibold text-dark mb-2">
            Company Information
          </h6>
          <p className="text-muted mb-1">
            Company: <strong>{companyName}</strong>
          </p>
          <p className="text-muted mb-0">
            Status: <span className="badge bg-success">Active</span>
          </p>
        </div>
      </div>

      {/* ================= ACTION CARDS ================= */}
      <div className="row g-4">

        {/* Projects */}
        <div className="col-md-4">
          <div className="border rounded p-4 h-100 bg-white">
            <h5 className="fw-semibold text-dark mb-2">
              Projects
            </h5>
            <p className="text-muted small mb-4">
              Create, update, and manage projects
            </p>
            <Link
              to="/user/projects"
              className="btn btn-dark w-100"
            >
              Manage Projects
            </Link>
          </div>
        </div>

        {/* Change History */}
        <div className="col-md-4">
          <div className="border rounded p-4 h-100 bg-white">
            <h5 className="fw-semibold text-dark mb-2">
              Change History
            </h5>
            <p className="text-muted small mb-4">
              View project and activity updates
            </p>
            <Link
              to="/user/history"
              className="btn btn-dark w-100"
            >
              View History
            </Link>
          </div>
        </div>

        {/* ================= LIVE ACTIVITY (SignalR) ================= */}
        <div className="col-md-4">
          <div
            className="border rounded p-4 h-100"
            style={{
              backgroundColor: "#1f1f1f",
              color: "#ffffff"
            }}
          >
            <h5 className="fw-semibold mb-2">
              Live Activity
            </h5>

            {/* Online users */}
            <div className="mb-3">
              <span className="badge bg-light text-dark">
                {onlineCount} online
              </span>
            </div>

            {/* Live feed */}
            <div
              style={{
                maxHeight: "220px",
                overflowY: "auto",
                fontSize: "13px"
              }}
            >
              {audits.length === 0 && (
                <div style={{ color: "#aaa" }}>
                  Waiting for activity…
                </div>
              )}

             {audits.map((item, index) => (
  <div
    key={index}
    style={{
      borderBottom: "1px solid #333",
      paddingBottom: "6px",
      marginBottom: "6px"
    }}
  >
    <strong>{item.changedByEmail}</strong>{" "}
    {item.changeType}{" "}
    <strong>{item.projectName}</strong>
  </div>
))}

            </div>
          </div>
        </div>

      </div>
    </div>
  );
}

export default CompanyUserDashboard;

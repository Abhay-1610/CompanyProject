import { Link } from "react-router-dom";
import { useEffect } from "react";
import { useDispatch, useSelector } from "react-redux";
import { fetchCompanies } from "../../features/companies/companiesSlice";

function SuperAdminDashboard() {

  const dispatch = useDispatch();
  const { list: companies, loading } = useSelector(
    (state) => state.companies 
  );


  useEffect(() => {
    dispatch(fetchCompanies());
  }, [dispatch]);

  return (
    <div
      className="min-vh-100"
      style={{
        backgroundColor: "#FBF7F2",
        padding: "40px",
      }}
    >
      {/* Header */}
      <div className="mb-5">
        <h3 className="fw-bold text-dark mb-1">
          Super Admin Dashboard
        </h3>
        <p className="text-muted mb-0">
          Manage companies and administrators
        </p>
      </div>

      {/* Action Cards */}
      <div className="row g-4 mb-5">
        <div className="col-md-4">
          <div className="border rounded p-4 h-100 bg-white">
            <h5 className="fw-semibold text-dark mb-2">
              Companies
            </h5>
            <p className="text-muted small mb-4">
              Register, update, and manage companies in the system.
            </p>
            <Link to="/super-admin/companies" className="btn btn-dark">
              Manage Companies
            </Link>
          </div>
        </div>

        <div className="col-md-4">
          <div className="border rounded p-4 h-100 bg-white">
            <h5 className="fw-semibold text-dark mb-2">
              Watch All Companies Audit Logs
            </h5>
            <p className="text-muted small mb-4">
              View history of operations done by anyone .
            </p>
            <Link to="/super-admin/history" className="btn btn-dark">
              All Audit Logs
            </Link>
          </div>
        </div>

        <div className="col-md-4">
          <div className="border rounded p-4 h-100 bg-white">
            <h5 className="fw-semibold text-dark mb-2">
              System Overview
            </h5>
            <p className="text-muted small mb-4">
              View system activity and audit information.
            </p>
            <button className="btn btn-outline-dark" disabled>
              Coming Soon
            </button>
          </div>
        </div>
      </div>

      {/* ================= COMPANY LIST ================= */}
      <div className="bg-white border rounded p-4">
        <h5 className="fw-semibold text-dark mb-3">
          Companies
        </h5>
        <p className="text-muted small mb-3">
          Select a company to view its users
        </p>

        {/* 🔹 Optional loading indicator */}
        {loading && (
          <p className="text-muted small">Loading companies...</p>
        )}

        <table className="table align-middle mb-0">
          <thead>
            <tr className="text-muted small">
              <th>Company Name</th>
              <th>Status</th>
              <th className="text-end">Action</th>
            </tr>
          </thead>

          <tbody>
            {companies.length === 0 && !loading ? (
              <tr>
                <td colSpan="3" className="text-center text-muted py-4">
                  No companies found
                </td>
              </tr>
            ) : (
              companies.map((company) => (
                <tr key={company.companyId}>
                  <td className="fw-semibold text-dark">
                    {company.companyName}
                  </td>

                  <td>
                    {company.isActive ? (
                      <span className="badge bg-success">
                        Active
                      </span>
                    ) : (
                      <span className="badge bg-secondary">
                        Inactive
                      </span>
                    )}
                  </td>

                  <td className="text-end">
  <Link
  to={`/super-admin/projects?companyId=${company.companyId}`}
  className="btn btn-sm btn-outline-dark me-2"
>
  Projects
</Link>


                    <Link
                      to={`/super-admin/users?companyId=${company.companyId}`}
                      className="btn btn-sm btn-outline-dark"
                    >
                      View Users
                    </Link>
                  </td>
                </tr>
              ))
            )}
          </tbody>
        </table>
      </div>
    </div>
  );
}

export default SuperAdminDashboard;

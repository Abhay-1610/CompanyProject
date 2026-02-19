import React, { useEffect, useState } from "react";
import baseUrl from "../../services/BaseUrl";

function CompanyAdminChangeHistory() {
  const [list, setList] = useState([]);
  const [loading, setLoading] = useState(false);

  /* =============================
     FETCH COMPANY AUDIT HISTORY
     (COMPANY ADMIN)
  ============================== */
  useEffect(() => {
    const fetchChangeHistory = async () => {
      try {
        setLoading(true);
        const res = await baseUrl.get("/change-history");
        setList(res.data);
      } catch (err) {
        console.error("Failed to load change history", err);
      } finally {
        setLoading(false);
      }
    };

    fetchChangeHistory();
  }, []);

  /* =============================
     SAFE JSON PARSER
  ============================== */
  const parseJsonSafe = (json) => {
    try {
      return json ? JSON.parse(json) : null;
    } catch {
      return null;
    }
  };

  return (
    <div
      className="min-vh-100"
      style={{ backgroundColor: "#FBF7F2", padding: "40px" }}
    >
      {/* Header */}
      <div className="mb-4">
        <h3 className="fw-bold text-dark mb-1">Change History</h3>
        <p className="text-muted mb-0">
          Audit log of all activities in your company
        </p>
      </div>

      {/* Table */}
      <div className="bg-white border rounded p-4">
        {loading && (
          <p className="text-muted small">Loading audit records...</p>
        )}

        <table className="table table-sm align-middle mb-0">
          <thead>
            <tr className="text-muted small">
              <th>Date & Time</th>
              <th>Changed By</th>
              <th>Action</th>
              <th>Summary</th>
            </tr>
          </thead>

          <tbody>
            {!loading && list.length === 0 ? (
              <tr>
                <td colSpan="4" className="text-center text-muted py-4">
                  No audit records available
                </td>
              </tr>
            ) : (
              list.map((item) => {
                const newData = parseJsonSafe(item.newData);

                return (
                  <tr key={item.changeId}>
                    {/* Date */}
                    <td className="text-muted">
                      {new Date(item.changedAt).toLocaleString()}
                    </td>

                    {/* User */}
                    <td className="fw-semibold text-dark">
                      {item.changedByEmail}
                    </td>

                    {/* Action */}
                    <td>
                      <span
                        className={`badge ${
                          item.changeType === "Create"
                            ? "bg-success"
                            : item.changeType === "Update"
                            ? "bg-warning text-dark"
                            : "bg-danger"
                        }`}
                      >
                        {item.changeType}
                      </span>
                    </td>

                    {/* Summary */}
                   <td className="text-muted small">
  {item?.projectName ? (
    <>
      <div>
        <strong>Project:</strong> {item.projectName}
      </div>
      <div>
        <strong>Action:</strong>{" "}
        <span className="text-capitalize">
          {item.changeType.toLowerCase()}
        </span>
      </div>
    </>
  ) : (
    "Record changed"
  )}
</td>

                  </tr>
                );
              })
            )}
          </tbody>
        </table>
      </div>
    </div>
  );
}

export default CompanyAdminChangeHistory;

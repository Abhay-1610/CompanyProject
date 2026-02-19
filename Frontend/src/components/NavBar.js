import React from "react";/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
import { Link, useNavigate } from "react-router-dom";
import { useDispatch, useSelector } from "react-redux";
import { clearAuth } from "../features/auth/authSlice";
import { stopSignalR } from "../services/signalRService";

function NavBar() {
  const dispatch = useDispatch();
  const navigate = useNavigate();

  const { userId, role } = useSelector((state) => state.auth);
  const userName = sessionStorage.getItem("userName");

  /* ============================
     BRAND CLICK (SAFE)
  ============================ */
  const handleClick = () => {
    if (!userId) {
      navigate("/login");
    } else if (role === "SuperAdmin") {
      navigate("/super-admin/dashboard");
    } else if (role === "CompanyAdmin") {
      navigate("/company-admin/dashboard");
    } else if (role === "CompanyUser") {
      navigate("/user/dashboard");
    } else {
      navigate("/login");
    }
  };

  /* ============================
     LOGOUT
  ============================ */
  const handleLogout = () => {
    stopSignalR();           // stop SignalR
    dispatch(clearAuth());   // clear auth state
  };

  return (
    <nav className="navbar navbar-expand-lg navbar-dark bg-dark border-bottom border-secondary">
      <div className="container-fluid">

        {/* LEFT */}
        <div className="d-flex align-items-center gap-4">
          <span
            className="navbar-brand fw-semibold mb-0"
            onClick={handleClick}
          >
            CompanyProject
          </span>

          {role === "SuperAdmin" && (
            <Link to="/super-admin/dashboard" className="nav-link text-light px-0">
              Super Admin
            </Link>
          )}

          {role === "CompanyAdmin" && (
            <Link to="/company-admin/dashboard" className="nav-link text-light px-0">
              Company Admin
            </Link>
          )}

          {role === "CompanyUser" && (
            <Link to="/user/dashboard" className="nav-link text-light px-0">
              Company User
            </Link>
          )}
        </div>

        {/* RIGHT */}
        <div className="ms-auto d-flex align-items-center gap-3">
          {userId && (
            <span className="text-white small m-2 p-2">
              Logged in as – <strong>{userName}</strong>
            </span>
          )}

          {!userId ? (
            <Link to="/login" className="btn btn-outline-light btn-sm">
              Login
            </Link>
          ) : (
            <button
              className="btn btn-outline-light btn-sm"
              onClick={handleLogout}
            >
              Logout
            </button>
          )}
        </div>

      </div>
    </nav>
  );
}

export default NavBar;

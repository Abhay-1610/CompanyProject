import { Routes, Route, Navigate } from "react-router-dom";
import { ROUTES } from "./Routes";
import ProtectedRoute from "./ProtectedRoute";

function Navigation() {
  return (
    <Routes>

      {/* ========== Common ========== */}
      <Route path={ROUTES.login.path} element={<ROUTES.login.component />}/>
      <Route path={ROUTES.unauthorized.path} element={<ROUTES.unauthorized.component />}/>

      {/* ========== SUPER ADMIN ========== */}
      <Route path={ROUTES.superAdminDashboard.path} element=
        {<ProtectedRoute allowedRoles={["SuperAdmin"]}>
            <ROUTES.superAdminDashboard.component />
        </ProtectedRoute>}
     />

<Route path={ROUTES.superAdminCompanies.path}  element={
    <ProtectedRoute allowedRoles={["SuperAdmin"]}>
      <ROUTES.superAdminCompanies.component />
    </ProtectedRoute>
  }
/>

<Route path={ROUTES.superAdminUsers.path}  element={
    <ProtectedRoute allowedRoles={["SuperAdmin"]}>
      <ROUTES.superAdminUsers.component />
    </ProtectedRoute>
  }
/>

<Route
  path={ROUTES.superAdminHistory.path}  element={
    <ProtectedRoute allowedRoles={["SuperAdmin"]}>
      <ROUTES.superAdminHistory.component />
    </ProtectedRoute>
  }
/>

<Route
  path={ROUTES.superAdminProjects.path}  element={
    <ProtectedRoute allowedRoles={["SuperAdmin"]}>
      <ROUTES.superAdminProjects.component />
    </ProtectedRoute>
  }
/>


      {/* ========== COMPANY ADMIN ========== */}
     <Route
  path={ROUTES.companyAdminDashboard.path} element={
    <ProtectedRoute allowedRoles={["CompanyAdmin"]}>
      <ROUTES.companyAdminDashboard.component />
    </ProtectedRoute>
  }
/>

<Route
  path={ROUTES.companyAdminUsers.path} element={
    <ProtectedRoute allowedRoles={["CompanyAdmin"]}>
      <ROUTES.companyAdminUsers.component />
    </ProtectedRoute>
  }
/>

<Route
  path={ROUTES.companyAdminAudit.path} element={
    <ProtectedRoute allowedRoles={["CompanyAdmin"]}>
      <ROUTES.companyAdminAudit.component />
    </ProtectedRoute>
  }
/>

<Route
  path={ROUTES.companyAdminProject.path} element={
    <ProtectedRoute allowedRoles={["CompanyAdmin"]}>
      <ROUTES.companyAdminProject.component />
    </ProtectedRoute>
  }
      />
      
      {/* ========== COMPANY User ========== */}

<Route
  path={ROUTES.companyUserDashboard.path} element={
    <ProtectedRoute allowedRoles={["CompanyUser"]}>
      <ROUTES.companyUserDashboard.component />
    </ProtectedRoute>
  }
/>

<Route
  path={ROUTES.companyUserProjects.path} element={
    <ProtectedRoute allowedRoles={["CompanyUser"]}>
      <ROUTES.companyUserProjects.component />
    </ProtectedRoute>
  }
/>

<Route
  path={ROUTES.companyUserHistory.path} element={
    <ProtectedRoute allowedRoles={["CompanyUser"]}>
      <ROUTES.companyUserHistory.component />
    </ProtectedRoute>
  }
/>


      {/* ========== FALLBACK ========== */}
      <Route
        path="*"
        element={<Navigate to={ROUTES.unauthorized.path} />}
      />

    </Routes>
  );
}

export default Navigation;

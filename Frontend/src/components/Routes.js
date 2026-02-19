import Login from "../pages/common/Login";
import Unauthorized from "../pages/common/Unauthorized";
import CompanyAdminChangeHistory from "../pages/companyAdmin/CompanyAdminChangeHistory";
import CompanyAdminDashboard from "../pages/companyAdmin/CompanyAdminDashboard";
import CompanyAdminProject from "../pages/companyAdmin/CompanyAdminProject";
import CompanyAdminUsers from "../pages/companyAdmin/CompanyAdminUsers";
import CompanyUserDashboard from "../pages/companyUser/CompanyUserDashboard";
import CompanyUserHistory from "../pages/companyUser/CompanyUserHistory";
import CompanyUserProjects from "../pages/companyUser/CompanyUserProjects";
import AllAudits from "../pages/superAdmin/AllAudits";
import SuperAdminCompanies from "../pages/superAdmin/Companies";
import SuperAdminDashboard from "../pages/superAdmin/SuperAdminDashboard";
import SuperAdminProjects from "../pages/superAdmin/SuperAdminProjects";
import SuperAdminUsers from "../pages/superAdmin/Users";

export const ROUTES = {
  login: {
    path: "/login",
    component: Login,
  },
  unauthorized: {
    path:"/unauthorized",
  component: Unauthorized,
},

// super admin
superAdminDashboard: {
  path: "/super-admin/dashboard",
  component: SuperAdminDashboard,
},

superAdminCompanies: {
  path: "/super-admin/companies",
  component: SuperAdminCompanies,
},

superAdminUsers: {
  path: "/super-admin/users",
  component: SuperAdminUsers,
  },
  superAdminProjects: {
  path: "/super-admin/projects",
  component: SuperAdminProjects,
  },
superAdminHistory: {
  path: "/super-admin/history",
  component: AllAudits,
  },


    
 // company admin
companyAdminDashboard: {
  path: "/company-admin/dashboard",
  component: CompanyAdminDashboard,
},

companyAdminUsers: {
  path: "/company-admin/users",
  component: CompanyAdminUsers,
},

companyAdminAudit: {
  path: "/company-admin/change-history",
  component: CompanyAdminChangeHistory,
  },
    companyAdminProject: {
  path: "/company-admin/project",
  component: CompanyAdminProject,
    },

 // ================= COMPANY USER =================
  companyUserDashboard: {
    path: "/user/dashboard",
    component: CompanyUserDashboard,
  },

  companyUserProjects: {
    path: "/user/projects",
    component: CompanyUserProjects,
  },
  companyUserHistory: {
    path: "/user/history",
    component: CompanyUserHistory,
  },

};

import { combineReducers } from "@reduxjs/toolkit";
import authReducer, { clearAuth } from "../features/auth/authSlice";
import usersReducer from "../features/users/usersSlice";
import companiesReducer from "../features/companies/companiesSlice";
import projectsReducer from "../features/projects/projectsSlice";
import signalRReducer from "../features/signalR/signalRSlice";

const appReducer = combineReducers({
  auth: authReducer,
  users: usersReducer,
  companies: companiesReducer,
  projects: projectsReducer,
  signalR: signalRReducer 
});

const rootReducer = (state, action) => {
  // Reset entire store on logout
  if (action.type === clearAuth.type) {
    state = undefined;
  }

  return appReducer(state, action);
};

export default rootReducer;

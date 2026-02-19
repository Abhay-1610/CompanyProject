import { createSlice } from "@reduxjs/toolkit";


const initialState = {
  accessToken: sessionStorage.getItem("accessToken"),
  userId: sessionStorage.getItem("userId"),
  role: sessionStorage.getItem("role"),
  companyId: sessionStorage.getItem("companyId"),
  companyName: sessionStorage.getItem("companyName")
};
 

const authSlice = createSlice({
  name: "auth",
  initialState,
  reducers: {
    setAuthContext(state, action) {
      state.accessToken = action.payload.accessToken;
      state.userId = action.payload.userId;
      state.role = action.payload.role;
      state.companyId = action.payload.companyId;
      state.companyName = action.payload.companyName;
    },
    clearAuth(state) {
      state.accessToken = null;
      state.userId = null;
      state.role = null;
      state.companyId = null;
      state.companyName = null;
      sessionStorage.clear();
    }
  }
});

export const { setAuthContext, clearAuth } = authSlice.actions;
export default authSlice.reducer;

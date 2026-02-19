import { createSlice } from "@reduxjs/toolkit";

const signalRSlice = createSlice({
  name: "signalR",
  initialState: {
    audits: [],
    onlineCount: 0
  },
  reducers: {
    addAuditItem(state, action) {
      state.audits.unshift(action.payload); 
    },
    setOnlineCount(state, action) {
      state.onlineCount = action.payload;
    },
    setConnected(state, action) {
      state.connected = action.payload;
    }
  }
});

export const { addAuditItem, setOnlineCount,setConnected } = signalRSlice.actions;
export default signalRSlice.reducer;

import { createSlice, createAsyncThunk } from "@reduxjs/toolkit";
import baseUrl from "../../services/BaseUrl";

/* ============================
   THUNKS
============================ */

// GET users by company
export const fetchUsersByCompany = createAsyncThunk(
  "users/fetchByCompany",
  async (companyId) => {
    const res = await baseUrl.get(`/users/company/${companyId}`);
    return res.data; // List<UserDto>
  }
);

// CREATE user (backend returns UserDto)
export const createUser = createAsyncThunk(
  "users/create",
  async (payload) => {
    const res = await baseUrl.post("/users", payload);
    return res.data; // UserDto
  }
);

// UPDATE user (backend returns UserDto)
export const updateUser = createAsyncThunk(
  "users/update",
  async ({ userId, email,role }) => {
    const res = await baseUrl.put(`/users/${userId}`, { email ,role});
    return res.data; // UserDto
  }
);

// DELETE user
export const deleteUser = createAsyncThunk(
  "users/delete",
  async (userId) => {
    await baseUrl.delete(`/users/${userId}`);
    return userId;
  }
);

// TOGGLE block (backend returns NoContent)
export const toggleBlockUser = createAsyncThunk(
  "users/toggleBlock",
  async (userId) => {
    await baseUrl.post(`/users/${userId}`);
    return userId;
  }
);
const usersSlice = createSlice({
  name: "users",
  initialState: {
    list: []
  },
  reducers: {
    clearUsers(state) {
      state.list = [];
    }
  },
  extraReducers: (builder) => {
    builder

      /* FETCH */
      .addCase(fetchUsersByCompany.fulfilled, (state, action) => {
        state.list = action.payload;
      })

      /* CREATE */
      .addCase(createUser.fulfilled, (state, action) => {
        state.list.push(action.payload); // full UserDto
      })

      /* UPDATE */
      .addCase(updateUser.fulfilled, (state, action) => {
        const index = state.list.findIndex(
          u => u.id === action.payload.id
        );
        if (index !== -1) {
          state.list[index] = action.payload; // replace with server truth
        }
      })

      /* DELETE */
      .addCase(deleteUser.fulfilled, (state, action) => {
        state.list = state.list.filter(
          u => u.id !== action.payload
        );
      })

      /* TOGGLE BLOCK (optimistic, backend returns nothing) */
      .addCase(toggleBlockUser.fulfilled, (state, action) => {
        const user = state.list.find(
          u => u.id === action.payload
        );
        if (user) {
          user.isBlocked = !user.isBlocked;
        }
      });
  }
});

export const { clearUsers } = usersSlice.actions;
export default usersSlice.reducer;

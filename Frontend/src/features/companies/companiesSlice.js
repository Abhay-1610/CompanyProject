import { createSlice, createAsyncThunk } from "@reduxjs/toolkit";
import baseUrl from "../../services/BaseUrl";

/* ============================
   THUNKS
============================ */

// GET companies
export const fetchCompanies = createAsyncThunk(
  "companies/fetch",
  async () => {
    const res = await baseUrl.get("/companies");
    return res.data; // List<CompanyDto>
  }
);

// CREATE company (backend returns CompanyDto)
export const createCompany = createAsyncThunk(
  "companies/create",
  async (payload) => {
    const res = await baseUrl.post("/companies", payload);
    return res.data; // CompanyDto
  }
);

// UPDATE company (backend returns CompanyDto)
export const updateCompany = createAsyncThunk(
  "companies/update",
  async ({ id, companyName }) => {
    const res = await baseUrl.put(`/companies/${id}`, { companyName });
    return res.data; // CompanyDto
  }
); 

// DELETE company
export const deleteCompany = createAsyncThunk(
  "companies/delete",
  async (id, { rejectWithValue }) => {
    try {
      await baseUrl.delete(`/companies/${id}`);
    return id;
    }
    catch (err) {
      return rejectWithValue(err.response?.data);
    }
  }
);
const companiesSlice = createSlice({
  name: "companies",
  initialState: {
    list: [],
    loading: false
  },
  reducers: {},
  extraReducers: (builder) => {
    builder

      /* FETCH */
      .addCase(fetchCompanies.pending, (state) => {
        state.loading = true;
      })
      .addCase(fetchCompanies.fulfilled, (state, action) => {
        state.loading = false;
        state.list = action.payload;
      })

      /* CREATE */
      .addCase(createCompany.fulfilled, (state, action) => {
        state.list.push(action.payload); // full CompanyDto
      })

      /* UPDATE */
      .addCase(updateCompany.fulfilled, (state, action) => {
        const index = state.list.findIndex(
          c => c.companyId === action.payload.companyId
        );
        if (index !== -1) {
          state.list[index] = action.payload; // replace with server truth
        }
      })

      /* DELETE */
      .addCase(deleteCompany.fulfilled, (state, action) => {
        state.list = state.list.filter(
          c => c.companyId !== action.payload
        );
      });
  }
});

export default companiesSlice.reducer;

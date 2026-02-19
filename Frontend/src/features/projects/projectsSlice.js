import { createSlice, createAsyncThunk } from "@reduxjs/toolkit";
import baseUrl from "../../services/BaseUrl";

/* ============================
   NORMALIZER
============================ */

const normalizeProject = (p) => ({
  projectId: p.ProjectId ?? p.projectId,
  projectName: p.ProjectName ?? p.projectName,
  description: p.Description ?? p.description,
  startDate: p.StartDate ?? p.startDate,
  endDate: p.EndDate ?? p.endDate,
  status: p.Status ?? p.status,
  isActive: p.IsActive ?? p.isActive
});

/* ============================
   THUNKS
============================ */

export const fetchProjectsByCompany = createAsyncThunk(
  "projects/fetchByCompany",
  async (companyId) => {
    const res = await baseUrl.get(`/projects?companyId=${companyId}`);
    return res.data;
  }
);

export const createProject = createAsyncThunk(
  "projects/create",
  async (payload) => {
    const res = await baseUrl.post("/projects", payload);
    return res.data;
  }
);

export const updateProject = createAsyncThunk(
  "projects/update",
  async ({ projectId, data }) => {
    const res = await baseUrl.put(`/projects/${projectId}`, data);
    return res.data;
  }
);

export const deleteProject = createAsyncThunk(
  "projects/delete",
  async (projectId, { rejectWithValue }) => {
    try {
      await baseUrl.delete(`/projects/${projectId}`);
      return projectId;
    } catch (err) {
      return rejectWithValue(err.response?.data);
    }
  }
);

/* ============================
   SLICE
============================ */

const projectsSlice = createSlice({
  name: "projects",
  initialState: {
    list: [],
    loading: false
  },

  reducers: {
    // 🔹 SignalR updates (idempotent)
    applyProjectAudit(state, action) {
      const audit = action.payload;
      const projectId = audit.projectId;
      const changeType = audit.changeType;

      let projectData = null;
      if (audit.newData) {
        try {
          projectData = JSON.parse(audit.newData);
        } catch {
          return;
        }
      }

      const project = projectData ? normalizeProject(projectData): null;

      if (changeType === "Create" && project) {
         const exists = state.list.some(
           p => p.projectId === project.projectId
         );
         if (!exists) {
           state.list.push(project);
         }
       }

      if (changeType === "Update" && project) {
        const index = state.list.findIndex(
          p => p.projectId === projectId
        );
        if (index !== -1) state.list[index] = project;
      }

      if (changeType === "Delete") {
        state.list = state.list.filter(
          p => p.projectId !== projectId
        );
      }
    }
  },

  extraReducers: (builder) => {
    builder

      /* FETCH */
      .addCase(fetchProjectsByCompany.pending, (state) => {
        state.loading = true;
      })
      .addCase(fetchProjectsByCompany.fulfilled, (state, action) => {
        state.loading = false;
        state.list = action.payload.map(normalizeProject);
      })

      /* CREATE */
      .addCase(createProject.fulfilled, (state, action) => {
  const project = normalizeProject(action.payload);

  const exists = state.list.some(
    p => p.projectId === project.projectId
  );

  if (!exists) {
    state.list.push(project);
  }
})


      /* UPDATE */
      .addCase(updateProject.fulfilled, (state, action) => {
        const updated = normalizeProject(action.payload);
        const index = state.list.findIndex(
          p => p.projectId === updated.projectId
        );
        if (index !== -1) state.list[index] = updated;
      })

      /* DELETE */
      .addCase(deleteProject.fulfilled, (state, action) => {
        state.list = state.list.filter(
          p => p.projectId !== action.payload
        );
      });
  }
});

export const { applyProjectAudit } = projectsSlice.actions;
export default projectsSlice.reducer;

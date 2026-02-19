import axios from "axios";
import { toast } from "react-toastify";

const baseUrl = axios.create({
  baseURL: "https://localhost:7122/api",
  headers: {
    "Content-Type": "application/json",
  },
});

// Request: attach JWT
baseUrl.interceptors.request.use(
  (config) => {
    const token = sessionStorage.getItem("accessToken");
    if (token) {
      config.headers.Authorization = `Bearer ${token}`;
    }
    return config;
  },
  (error) => Promise.reject(error)
);


// Response: handle errors properly
baseUrl.interceptors.response.use(
  (response) => response,

  async (error) => {
    const status = error.response?.status;
    const data = error.response?.data;
    const originalRequest = error.config;

    // 🔁 401 → try refresh token ONCE
    if (status === 401 && !originalRequest._retry) {
      originalRequest._retry = true;

      try {
        const accessToken =
          sessionStorage.getItem("accessToken");
        const refreshToken =
          sessionStorage.getItem("refreshToken");

        const response = await axios.post(
          "https://localhost:7122/api/auth/refresh-token",
          {
            accessToken,
            refreshToken,
          }
        );

        // save new tokens
        sessionStorage.setItem(
          "accessToken",
          response.data.accessToken
        );
        sessionStorage.setItem(
          "refreshToken",
          response.data.refreshToken
        );

        // retry original request
        originalRequest.headers.Authorization =
          "Bearer " + response.data.accessToken;

        return baseUrl(originalRequest);
      } catch {
        // refresh failed → fall through to existing logic
      }
    }

    // 🚫 Forbidden (unchanged)
    if (status === 403) {
      toast.error(error.response?.data || "Forbiddenn");
      return Promise.reject(error);
    }

    // 🚫 Unauthorized (only if refresh failed)
    if (status === 401) {
      toast.error(error.response?.data || "Unauthorized");
      return Promise.reject(error);
    }

    // 🚫 Validation errors (unchanged)
    if (status === 400 && Array.isArray(data)) {
      toast.error(data.join("\n"));
      return Promise.reject(data);
    }

    // 🚫 Server error (unchanged)
    if (status === 500) {
      toast.error(error.response?.data || "Something went wrong" );
      return Promise.reject(error);
    }

    return Promise.reject(error);
  }
);


export default baseUrl;

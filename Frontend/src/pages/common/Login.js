import React from "react";
import { useNavigate } from "react-router-dom";
import { jwtDecode } from "jwt-decode";
import { useDispatch } from "react-redux";
import { setAuthContext } from "../../features/auth/authSlice";
import baseUrl from "../../services/BaseUrl";

// 🔹 Formik + Yup
import { Formik } from "formik";
import * as Yup from "yup";

// 🔹 Toastify
import { toast } from "react-toastify";
import "react-toastify/dist/ReactToastify.css";

function Login() {
  const navigate = useNavigate();
  const dispatch = useDispatch();

  /* =============================
     FORM VALIDATION SCHEMA
  ============================== */
  const validationSchema = Yup.object({
    email: Yup.string()
      .email("Invalid email")
      .required("Email is required"),
    password: Yup.string()
      .required("Password is required")
  });

  /* =============================
     LOGIN HANDLER
  ============================== */
  const handleLogin = async (values, { setSubmitting }) => {
    try {
      const response = await baseUrl.post("/auth/login", {
        email: values.email,
        password: values.password
      });

      const { accessToken, refreshToken } = response.data;

      sessionStorage.setItem("accessToken", accessToken);
      sessionStorage.setItem("refreshToken", refreshToken);

      const decoded = jwtDecode(accessToken);

      const role = decoded["role"];
      const companyId = decoded["companyId"];
      const userId = decoded["userId"];
      const companyName = decoded["companyName"];
      const userName = decoded["email"];

      sessionStorage.setItem("role", role);
      sessionStorage.setItem("companyId", companyId);
      sessionStorage.setItem("companyName", companyName);
      sessionStorage.setItem("userId", userId);
      sessionStorage.setItem("userName", userName);

      dispatch(
        setAuthContext({
          accessToken,
          userId,
          role,
          companyId,
          companyName
        })
      );

      toast.success("Login successful");

      if (role === "SuperAdmin") {
        navigate("/super-admin/dashboard");
      } else if (role === "CompanyAdmin") {
        navigate("/company-admin/dashboard");
      } else {
        navigate("/user/dashboard");
      }
    } catch (error) {
    } finally {
      setSubmitting(false);
    }
  };

  return (
    <Formik
      initialValues={{ email: "", password: "" }}
      validationSchema={validationSchema}
      onSubmit={handleLogin}
    >
      {({
        values,
        errors,
        touched,
        handleChange,
        handleSubmit,
        isSubmitting
      }) => (
        <form onSubmit={handleSubmit}>
          <div
            className="d-flex align-items-center justify-content-center"
            style={{
              minHeight: "100vh",
              backgroundColor: "#dcdcdc",
              padding: "40px"
            }}
          >
            <div
              className="d-flex rounded shadow-lg"
              style={{
                maxWidth: "900px",
                width: "100%",
                overflow: "hidden",
                backgroundColor: "#FBF7F2"
              }}
            >
              {/* LEFT INFO SECTION */}
              <div
                className="d-none d-md-flex align-items-center justify-content-center"
                style={{
                  width: "40%",
                  backgroundColor: "#F3EDE4",
                  padding: "30px"
                }}
              >
                <div className="text-center">
                  <h2 className="fw-semibold text-dark">Welcome</h2>
                  <img
                    src="https://picsum.photos/400/300"
                    alt="welcome"
                    className="img-fluid rounded mb-3"
                    style={{ maxHeight: "220px" }}
                  />
                  <p className="fst-italic text-muted">
                    Secure access to your<br />company workspace
                  </p>
                </div>
              </div>

              {/* RIGHT LOGIN CARD */}
              <div className="p-5" style={{ width: "60%" }}>
                <h4 className="fw-bold text-center mb-2 text-dark">
                  CompanyProject
                </h4>

                <p className="text-center text-muted mb-4">
                  Sign in to continue
                </p>

                {/* EMAIL */}
                <div className="mb-3">
                  <label className="form-label text-dark">Email</label>
                  <input
                    type="email"
                    name="email"
                    className="form-control"
                    value={values.email}
                    onChange={handleChange}
                  />

                  {/* 🔒 Reserved space prevents layout shift */}
                  <div style={{ minHeight: "24px" }}>
                    {touched.email && errors.email && (
                      <small className="text-danger">{errors.email}</small>
                    )}
                  </div>
                </div>

                {/* PASSWORD */}
                <div className="mb-4">
                  <label className="form-label text-dark">Password</label>
                  <input
                    type="password"
                    name="password"
                    className="form-control"
                    value={values.password}
                    onChange={handleChange}
                  />

                  {/* 🔒 Reserved space prevents layout shift */}
                  <div style={{ minHeight: "24px" }}>
                    {touched.password && errors.password && (
                      <small className="text-danger">{errors.password}</small>
                    )}
                  </div>
                </div>

                <button
                  type="submit"
                  className="btn btn-dark w-100 mb-3"
                  disabled={isSubmitting}
                >
                  Sign In
                </button>

                <div className="text-center">
                  <span className="text-muted small">
                    Authorized personnel only
                  </span>
                </div>
              </div>
            </div>
          </div>
        </form>
      )}
    </Formik>
  );
}

export default Login;
 
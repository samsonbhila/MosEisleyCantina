import React, { useState } from "react";
import Modal from "../components/Modal";
import { GoogleLogin } from "@react-oauth/google";
import { loginUser } from "../services/apiService";
import { jwtDecode } from "jwt-decode";
import "../styles/Login.css";

const Login = () => {
  const [email, setEmail] = useState("");
  const [password, setPassword] = useState("");
  const [modalOpen, setModalOpen] = useState(false);
  const [modalContent, setModalContent] = useState({ title: "", message: "" });
  const [loading, setLoading] = useState(false);

  const backendUrl = process.env.REACT_APP_BACKEND_URL || "http://localhost:8080";

  // Handle regular login with email and password
  const handleLogin = async (e) => {
    e.preventDefault();
    setLoading(true);
    try {
      const response = await loginUser({ email, password });
      const { token } = response.data;
      handleTokenProcessing(token);
    } catch (err) {
      console.error(err);
      const errorMessage =
        err.response?.data?.message || "Invalid email or password.";
      setModalContent({ title: "Error", message: errorMessage });
      setModalOpen(true);
    } finally {
      setLoading(false);
    }
  };

  // Process the token received (JWT token)
  const handleTokenProcessing = (token, isGoogleLogin = false) => {
    try {
      const decodedToken = jwtDecode(token);

      if (isGoogleLogin) {
        // Skip role check for Google users and redirect them to the dashboard
        localStorage.setItem("token", token);
        setModalContent({ title: "Success", message: "Login successful! Redirecting..." });
        setModalOpen(true);
        setTimeout(() => (window.location.href = "/dashboard"), 2000);
      } else {
        // Handle regular login with role-based access control
        const role = decodedToken["http://schemas.microsoft.com/ws/2008/06/identity/claims/role"];
        localStorage.setItem("token", token);
        localStorage.setItem("role", role);

        if (role.toLowerCase() === "admin".toLowerCase()) {
          setModalContent({ title: "Success", message: "Login successful! Redirecting..." });
          setModalOpen(true);
          setTimeout(() => (window.location.href = "/dashboard"), 2000);
        } else {
          setModalContent({
            title: "Access Denied",
            message: "You are not authorized to access this dashboard.",
          });
          setModalOpen(true);
        }
      }
    } catch (error) {
      console.error("Token processing error:", error);
      setModalContent({ title: "Error", message: "Invalid token received." });
      setModalOpen(true);
    }
  };

  // Handle Google Login Success
  const handleGoogleSuccess = async (credentialResponse) => {
    setLoading(true);
    console.log("Credential Response:", credentialResponse);
    try {
      const response = await fetch(
        `${backendUrl}/api/Auth/signin-google?token=${encodeURIComponent(
          credentialResponse.credential
        )}`,
        {
          method: "POST",
        }
      );
      if (response.ok) {
        const data = await response.json();
        handleTokenProcessing(data.token, true);
      } else {
        const errorText = await response.text();
        console.error("Error:", errorText);
        setModalContent({
          title: "Error",
          message: errorText || "Google login failed.",
        });
        setModalOpen(true);
      }
    } catch (error) {
      console.error("Google login failed", error);
      const errorMessage =
        error.response?.data?.message || "Google login failed.";
      setModalContent({ title: "Error", message: errorMessage });
      setModalOpen(true);
    } finally {
      setLoading(false);
    }
  };

  // Handle Google Login Failure
  const handleGoogleFailure = () => {
    setModalContent({ title: "Error", message: "Google login failed." });
    setModalOpen(true);
  };

  return (
    <div className="login-container">
      <h2>Login</h2>
      <form onSubmit={handleLogin}>
        <div>
          <label>Email:</label>
          <input
            type="email"
            value={email}
            onChange={(e) => setEmail(e.target.value)}
            required
            aria-label="Email"
          />
        </div>
        <div>
          <label>Password:</label>
          <input
            type="password"
            value={password}
            onChange={(e) => setPassword(e.target.value)}
            required
            aria-label="Password"
          />
        </div>
        <button type="submit" disabled={loading}>
          {loading ? "Logging in..." : "Login"}
        </button>
      </form>

      <div style={{ marginTop: "20px" }}>
        <h3>Or Login with Google:</h3>
        <GoogleLogin onSuccess={handleGoogleSuccess} onError={handleGoogleFailure} />
      </div>

      <Modal
        isOpen={modalOpen}
        title={modalContent.title}
        message={modalContent.message}
        onClose={() => setModalOpen(false)}
      />
    </div>
  );
};

export default Login;

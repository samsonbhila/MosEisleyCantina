import React from "react";
import ReactDOM from "react-dom/client";
import { GoogleOAuthProvider } from "@react-oauth/google";
import "./index.css";
import "./styles.css";
import App from "./App";
import reportWebVitals from "./reportWebVitals";

const CLIENT_ID = "1056488017525-okvhqj22n2g1f5t5dq11t7c2hgmv8jd9.apps.googleusercontent.com"; // Replace with your actual Google Client ID

const root = ReactDOM.createRoot(document.getElementById("root"));
root.render(
  <React.StrictMode>
    <GoogleOAuthProvider clientId={CLIENT_ID}>
      <App />
    </GoogleOAuthProvider>
  </React.StrictMode>
);

// If you want to start measuring performance in your app, pass a function
// to log results (for example: reportWebVitals(console.log))
// or send to an analytics endpoint. Learn more: https://bit.ly/CRA-vitals
reportWebVitals();

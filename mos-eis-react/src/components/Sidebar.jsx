import React from "react";
import "../styles/style.css"; // Adjust the path as needed for your CSS file

const Sidebar = () => {
  return (
    <aside>
      <div className="toggle">
        <div className="logo">
          <img src="../images/logo.png" alt="Logo" />
          <h2>
            TFG<span className="danger">LIMITED</span>
          </h2>
        </div>
        <div className="close" id="close-btn">
          <span className="material-icons-sharp">close</span>
        </div>
      </div>

      <div className="sidebar">
        <a href="#..">
          <span className="material-icons-sharp">dashboard</span>
          <h3>Dashboard</h3>
        </a>
        <a href="#..">
          <span className="material-icons-sharp">person_outline</span>
          <h3>Users</h3>
        </a>
        <a href="#..">
          <span className="material-icons-sharp">receipt_long</span>
          <h3>History</h3>
        </a>
        <a href="#.." className="active">
          <span className="material-icons-sharp">insights</span>
          <h3>Analytics</h3>
        </a>
        <a href="#..">
          <span className="material-icons-sharp">mail_outline</span>
          <h3>Tickets</h3>
          <span className="message-count">27</span>
        </a>
        <a href="#..">
          <span className="material-icons-sharp">inventory</span>
          <h3>Sale List</h3>
        </a>
        <a href="#..">
          <span className="material-icons-sharp">report_gmailerrorred</span>
          <h3>Reports</h3>
        </a>
        <a href="#..">
          <span className="material-icons-sharp">settings</span>
          <h3>Settings</h3>
        </a>
        <a href="#..">
          <span className="material-icons-sharp">add</span>
          <h3>New Login</h3>
        </a>
        <a href="/">
          <span className="material-icons-sharp">logout</span>
          <h3>Logout</h3>
        </a>
      </div>
    </aside>
  );
};

export default Sidebar;

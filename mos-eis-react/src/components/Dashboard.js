import React, { useState } from "react";
import Sidebar from "./Sidebar";
import MainContent from "./MainContent";
import RightSection from "./RightSection";
import "../styles/Dashboard.css";

const Dashboard = () => {
  // Global states
  const [sideMenuVisible, setSideMenuVisible] = useState(false);
  const [darkMode, setDarkMode] = useState(false);

  // Sample data (can come from an API in the future)
  const orders = [
    {
      productName: "Product 1",
      productNumber: "001",
      paymentStatus: "Paid",
      status: "Approved",
    },
    {
      productName: "Product 2",
      productNumber: "002",
      paymentStatus: "Unpaid",
      status: "Pending",
    },
    {
      productName: "Product 3",
      productNumber: "003",
      paymentStatus: "Refunded",
      status: "Declined",
    },
  ];

  // Handlers
  const toggleSideMenu = () => setSideMenuVisible(!sideMenuVisible);
  const toggleDarkMode = () => setDarkMode(!darkMode);

  return (
    <div className={darkMode ? "dark-mode-variables" : ""}>
      {/* Sidebar */}
      <Sidebar
        visible={sideMenuVisible}
        toggleVisibility={toggleSideMenu}
      />

      {/* Main Content */}
      <MainContent orders={orders} />

      {/* Right Section */}
      <RightSection
        toggleDarkMode={toggleDarkMode}
        darkMode={darkMode}
      />
    </div>
  );
};

export default Dashboard;

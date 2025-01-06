import React, { useState } from "react";
import Sidebar from "./Sidebar";
import MainContent from "./MainContent";
import RightSection from "./RightSection";
import "../styles/Dashboard.css";

const Dashboard = () => {
  const [sideMenuVisible, setSideMenuVisible] = useState(false);
  const [darkMode, setDarkMode] = useState(false);

  const orders = [
    {
      productName: "Sprite",
      productNumber: "001",
      paymentStatus: "Paid",
      status: "Approved",
    },
    {
      productName: "Coke",
      productNumber: "002",
      paymentStatus: "Unpaid",
      status: "Pending",
    },
    {
      productName: "Pepsi",
      productNumber: "003",
      paymentStatus: "Refunded",
      status: "Declined",
    },
  ];

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

import React from 'react';
import Sidebar from '../components/Sidebar';
import MainContent from '../components/MainContent';
import RightSection from '../components/RightSection';
//import '../styles/Dashboard.css';
import '../styles/style.css';

const Dashboard = () => {
  return (
    <div className="dashboard-container">
      <aside>
        <Sidebar />
      </aside>
       <main>
        <MainContent /> 
      </main>
     <section className="right-section">
        <RightSection /> 
      </section>
    </div>
  );
};

export default Dashboard;

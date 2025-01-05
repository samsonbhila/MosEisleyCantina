import React from "react";
import "../styles/MainContent.css"; 

const MainContent = () => {
  // Sample  orders
  const orders = [
    {
      productName: "Chicken",
      productNumber: "85743",
      paymentStatus: "Due",
      status: "Pending",
    },
    {
      productName: "Steak",
      productNumber: "97245",
      paymentStatus: "Refunded",
      status: "Declined",
    },
    {
      productName: "Taco",
      productNumber: "36452",
      paymentStatus: "Paid",
      status: "Active",
    },
  ];

  const users = [
    { image: "/images/profile-2.jpg", name: "Mick", time: "54 Min Ago" },
    { image: "/images/profile-3.jpg", name: "Amir", time: "3 Hours Ago" },
    { image: "/images/profile-4.jpg", name: "Auston", time: "6 Hours Ago" },
  ];

  return (
    <main className="main-content">
      <h1>Analytics</h1>

      {/* Sales, Visits, and Searches Cards */}
      <div className="analyse">
        <div className="sales">
          <div className="status">
            <div className="info">
              <h3>Total Sales</h3>
              <h1>R35,90</h1>
            </div>
            <div className="progresss">
              <svg>
                <circle cx="38" cy="38" r="36" />
              </svg>
              <div className="percentage">
                <p>+85%</p>
              </div>
            </div>
          </div>
        </div>

        <div className="visits">
          <div className="status">
            <div className="info">
              <h3>Total Visits</h3>
              <h1>152,800</h1>
            </div>
            <div className="progresss">
              <svg>
                <circle cx="38" cy="38" r="36" />
              </svg>
              <div className="percentage">
                <p>+70%</p>
              </div>
            </div>
          </div>
        </div>

        <div className="searches">
          <div className="status">
            <div className="info">
              <h3>Total Searches</h3>
              <h1>15,230</h1>
            </div>
            <div className="progresss">
              <svg>
                <circle cx="38" cy="38" r="36" />
              </svg>
              <div className="percentage">
                <p>+80%</p>
              </div>
            </div>
          </div>
        </div>
      </div>

      {/* New Users Section */}
      <section className="new-users">
        <h2>New Users</h2>
        <div className="user-list">
          {users.map((user, index) => (
            <div className="user" key={index}>
              <img src={user.image} alt={user.name} />
              <h3>{user.name}</h3>
              <p>{user.time}</p>
            </div>
          ))}
          <div className="user">
            <img src="./images/plus.png" alt="Add User" />
            <h3>More</h3>
            <p>New User</p>
          </div>
        </div>
      </section>

      {/* Recent Orders Section */}
      <section className="recent-orders">
        <h2>Recent Orders</h2>
        <table>
          <thead>
            <tr>
              <th>Customer Name</th>
              <th>Customer Number</th>
              <th>Payment</th>
              <th>Status</th>
              <th>Action</th>
            </tr>
          </thead>
          <tbody>
            {orders.map((order, index) => (
              <tr key={index}>
                <td>{order.productName}</td>
                <td>{order.productNumber}</td>
                <td>{order.paymentStatus}</td>
                <td>{order.status}</td>
                <td>
                  <button>Details</button>
                </td>
              </tr>
            ))}
          </tbody>
        </table>
        <a href="#..">Show All</a>
      </section>
    </main>
  );
};

export default MainContent;

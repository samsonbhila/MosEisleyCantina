import React, { useState, useEffect } from "react";
import "../styles/style.css";
import "../styles/RightSection.css";
import { getReviews } from "../services/apiService"; // Assuming you have the API function to fetch reviews
import {jwtDecode} from "jwt-decode"; // Import jwt-decode to decode the JWT token

const RightSection = () => {
  const [darkMode, setDarkMode] = useState(false);
  const [reviews, setReviews] = useState([]);
  const [loading, setLoading] = useState(true);
  const [userName, setUserName] = useState(""); // State for the logged-in user's name

  const toggleDarkMode = () => {
    setDarkMode(!darkMode);
    document.body.classList.toggle("dark-mode-variables", !darkMode);
  };

  useEffect(() => {
    // Get the JWT token from localStorage
    const token = localStorage.getItem("authToken"); // Assuming the token is stored as "authToken"
    if (token) {
      try {
        const decodedToken = jwtDecode(token); // Decode the token to extract user information
        const loggedInUser = decodedToken.userName; // Replace with the correct property from your token
        setUserName(loggedInUser); // Set the user's name
      } catch (error) {
        console.error("Invalid token:", error);
      }
    }

    // Fetch reviews from the API
    getReviews()
      .then((data) => {
        setReviews(data); // Set the reviews
        setLoading(false); // Stop loading when reviews are fetched
      })
      .catch((err) => {
        console.error("Error fetching reviews:", err);
        setLoading(false);
      });
  }, []); // Empty dependency array to run once on mount

  const renderStars = (rating) => {
    let stars = [];
    for (let i = 0; i < 5; i++) {
      stars.push(
        <span key={i} className="material-icons-sharp">
          {i < rating ? "star" : "star_border"}
        </span>
      );
    }
    return stars;
  };

  return (
    <div className="right-section">
      {/* Navigation Section */}
      <div className="nav">
        <button id="menu-btn">
          <span className="material-icons-sharp">menu</span>
        </button>
        <div className="dark-mode">
          <span
            className={`material-icons-sharp ${!darkMode ? "active" : ""}`}
            onClick={toggleDarkMode}
          >
            light_mode
          </span>
          <span
            className={`material-icons-sharp ${darkMode ? "active" : ""}`}
            onClick={toggleDarkMode}
          >
            dark_mode
          </span>
        </div>
        <div className="profile">
          <div className="info">
            <p>
              Hey, <b>{userName || "Guest"}</b> {/* Display the logged-in user's name */}
            </p>
            <small className="text-muted">Admin</small>
          </div>
          <div className="profile-photo">
            <img src="/images/profile-1.jpg" alt="Profile" />
          </div>
        </div>
      </div>

      {/* User Profile Section */}
      <div className="user-profile">
        <div className="logo">
          <img src="/images/logo.png" alt="Logo" />
          <h2>TFG</h2>
          <p>Clothing stores</p>
        </div>
      </div>

      {/* Reviews Section */}
      <div className="reminders">
        <div className="header">
          <h2>Reviews</h2>
          <span className="material-icons-sharp">notifications_none</span>
        </div>

        {loading ? (
          <div>Loading reviews...</div>
        ) : (
          <div className="reviews-container">
            {reviews.map((review) => (
              <div key={review.id} className="review-card">
                <div className="icon">
                  <span className="material-icons-sharp">volume_up</span>
                </div>
                <div className="content">
                  <div className="info">
                    <h3>{review.userName}</h3>
                    <small className="text-muted">
                      {new Date(review.createdAt).toLocaleString()}
                    </small>
                  </div>
                  <div className="rating">{renderStars(review.rating)}</div>
                  <p>{review.content}</p>
                  <span className="material-icons-sharp">more_vert</span>
                </div>
              </div>
            ))}
          </div>
        )}

        <div className="notification add-reminder">
          <div>
            <span className="material-icons-sharp">add</span>
            <h3>Add A review</h3>
          </div>
        </div>
      </div>
    </div>
  );
};

export default RightSection;

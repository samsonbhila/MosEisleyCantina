import axios from "axios";

const API_BASE_URL = "http://localhost:8080/api";

const apiClient = axios.create({
  baseURL: API_BASE_URL,
  headers: {
    "Content-Type": "application/json",
  },
});

// Login request
export const loginUser = async (data) => {
  try {
    const response = await apiClient.post("/Auth/login", data);
    
    // Log the full response to ensure it's correct
    console.log("API response:", response);

    // Store the token in localStorage
    localStorage.setItem("authToken", response.data.token);  // Assuming the token is in response.data.token

    // Return the response object (optional)
    return response;
  } catch (error) {
    console.error("Login error:", error);
    throw error;  // Throw the error so it can be handled in the component
  }
};

// Register request
export const registerUser = async (data) => {
  try {
    const response = await apiClient.post("/Auth/register", data);
    return response.data; // Return response data (e.g., success message)
  } catch (error) {
    // Handle any error that occurred
    if (error.response) {
      console.error("Error response:", error.response);
      throw new Error(`Registration failed: ${error.response.data.message || error.response.statusText}`);
    } else if (error.request) {
      console.error("Error request:", error.request);
      throw new Error("No response from server");
    } else {
      console.error("Error message:", error.message);
      throw new Error("An unknown error occurred");
    }
  }
};

// Fetch Reviews
export const getReviews = async () => {
  try {
    // Retrieve the token from localStorage
    const token = localStorage.getItem("authToken");

    if (!token) {
      throw new Error("No authentication token found. Please log in.");
    }

    // Set the Authorization header with the token
    const response = await apiClient.get("/reviews", {
      headers: {
        Authorization: `Bearer ${token}`,
      },
    });

    console.log("API response:", response);
    return response.data;
  } catch (error) {
    console.error("Error fetching reviews:", error);
    throw error;  // Throw the error so it can be handled in the component
  }
};

// Logout request (removes token from localStorage)
export const logoutUser = () => {
  // Remove the token from localStorage
  localStorage.removeItem("authToken");
};

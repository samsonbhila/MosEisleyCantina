import axios from "axios";

const API_BASE_URL = "http://localhost:8080/api";

const apiClient = axios.create({
  baseURL: API_BASE_URL,
  headers: {
    "Content-Type": "application/json",
  },
});

export const loginUser = async (data) => {
  try {
    const response = await apiClient.post("/Auth/login", data);
    console.log("API response:", response);

    // Store the token in localStorage
    localStorage.setItem("authToken", response.data.token);  

    return response;
  } catch (error) {
    console.error("Login error:", error);
    throw error;  
  }
};


export const registerUser = async (data) => {
  try {
    const response = await apiClient.post("/Auth/register", data);
    return response.data; 
  } catch (error) {
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
    const token = localStorage.getItem("authToken");

    if (!token) {
      throw new Error("No authentication token found. Please log in.");
    }

    const response = await apiClient.get("/reviews", {
      headers: {
        Authorization: `Bearer ${token}`,
      },
    });

    console.log("API response:", response);
    return response.data;
  } catch (error) {
    console.error("Error fetching reviews:", error);
    throw error;  
  }
};


export const logoutUser = () => {
  localStorage.removeItem("authToken");
};

export const getToken = () => localStorage.getItem("token");

export const isAuthenticated = () => {
  const token = getToken();
  if (!token) return false;

  try {
    // Decode token if needed or verify its structure
    const payload = JSON.parse(atob(token.split(".")[1])); // Decode payload
    return payload.exp > Date.now() / 1000; // Check expiration
  } catch {
    return false;
  }
};

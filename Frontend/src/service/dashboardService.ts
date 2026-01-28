import api from "./api";

export const dashboardService = {
  getDashboard: async () => {
    try {
      const response = await api.get("/dashboard"); // matches your backend route
      return response.data;
    } catch (error) {
      console.error("Error fetching dashboard data:", error);
      throw error;
    }
  },
};

import api from "./api";

export interface Permission {
  id: number;
  name: string;
}

export const permissionService = {
  getAll: () => api.get<Permission[]>("/Permissions"),
  getById: (id: number) => api.get<Permission>(`/Permissions/${id}`),
  create: (name: string) => api.post<Permission>("/Permissions", { name }),
  update: (id: number, name: string) => api.put<Permission>(`/Permissions/${id}`, { name }),
  delete: (id: number) => api.delete(`/Permissions/${id}`),
};

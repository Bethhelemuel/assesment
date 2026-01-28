import api from "./api";

export interface User {
  id: number;
  fullName: string;
  email: string;
  groups: { id: number; name: string }[];
}

export interface UserCreateUpdate {
  fullName: string;
  email: string;
  groupIds: number[];
}

export const userService = {
  getAll: () => api.get<User[]>("/Users"),
  getById: (id: number) => api.get<User>(`/Users/${id}`),
  create: (data: UserCreateUpdate) => api.post<User>("/Users", data),
  update: (id: number, data: UserCreateUpdate) => api.put<User>(`/Users/${id}`, data),
  delete: (id: number) => api.delete(`/Users/${id}`),
};

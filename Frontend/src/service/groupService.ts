import api from "./api";

export interface Permission {
  id: number;
  name: string;
  groups: any[];
}

export interface Group {
  id: number;
  name: string;
  users: any[];
  permissions: Permission[];
}

export interface GroupCreateRequest {
  name: string;
  userIds: number[];
  permissionIds: number[];
}

export interface GroupUpdateRequest {
  name: string;
  userIds: number[];
  permissionIds: number[];
}

export const groupService = {
  getAll: () => api.get<Group[]>("/Groups"),
  getById: (id: number) => api.get<Group>(`/Groups/${id}`),
  create: (name: string, permissionIds: number[] = [], userIds: number[] = []) => 
    api.post<Group>("/Groups", { name, userIds, permissionIds }),
  update: (id: number, name: string, permissionIds: number[] = [], userIds: number[] = []) => 
    api.put<Group>(`/Groups/${id}`, { name, userIds, permissionIds }),
  delete: (id: number) => api.delete(`/Groups/${id}`),
};

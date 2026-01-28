import { useState, useEffect } from "react";
import api from "../service/api";
import { Button } from "@/components/ui/button";
import {
  Dialog,
  DialogContent,
  DialogHeader,
  DialogTitle, 
  DialogTrigger,
  DialogFooter,
} from "@/components/ui/dialog";

import { UserDialog, UserTable, UserStats } from "@/components/users";

type Group = {
  id: number;
  name: string;
};

type User = {
  id: number;
  fullName: string;
  email: string;
  groups: Group[];
};

export default function Users() {
  const [users, setUsers] = useState<User[]>([]);
  const [availableGroups, setAvailableGroups] = useState<Group[]>([]);
  const [loading, setLoading] = useState(true);

  const [createOpen, setCreateOpen] = useState(false);
  const [editOpen, setEditOpen] = useState(false);
  const [deleteOpen, setDeleteOpen] = useState(false);

  const [form, setForm] = useState({ fullName: "", email: "" });
  const [selectedGroupIds, setSelectedGroupIds] = useState<number[]>([]);
  const [selectedUser, setSelectedUser] = useState<User | null>(null);

  // Fetch users and groups
  const fetchData = async () => {
    try {
      const [usersRes, groupsRes] = await Promise.all([
        api.get("/Users"),
        api.get("/Groups")
      ]);
      setUsers(usersRes.data);
      setAvailableGroups(groupsRes.data);
    } catch (error) {
      console.error("Failed to fetch data", error);
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => {
    fetchData();
  }, []);

  // CREATE
  const handleCreate = async () => {
    try {
      console.log("Creating user:", form.fullName, "with groups:", selectedGroupIds);
      const res = await api.post("/Users", { 
        fullName: form.fullName, 
        email: form.email,
        groupIds: selectedGroupIds 
      });
      console.log("Create response:", res.data);
      // Refresh data to get the latest from server
      await fetchData();
      setForm({ fullName: "", email: "" });
      setSelectedGroupIds([]);
      setCreateOpen(false);
    } catch (error) {
      console.error("Failed to create user", error);
    }
  };

  // EDIT
  const handleEdit = async () => {
    if (!selectedUser) return;
    try {
      console.log("Editing user:", selectedUser.id, "with data:", form, "groups:", selectedGroupIds);
      const res = await api.put(`/Users/${selectedUser.id}`, { 
        fullName: form.fullName, 
        email: form.email,
        groupIds: selectedGroupIds 
      });
      console.log("Edit response:", res.data);
      // Refresh data to get the latest from server
      await fetchData();
      setForm({ fullName: "", email: "" });
      setSelectedGroupIds([]);
      setSelectedUser(null);
      setEditOpen(false);
    } catch (error) {
      console.error("Failed to update user", error);
    }
  };

  // DELETE
  const handleDelete = async () => {
    if (!selectedUser) return;
    try {
      console.log("Deleting user:", selectedUser.id);
      await api.delete(`/Users/${selectedUser.id}`);
      // Refresh data to get the latest from server
      await fetchData();
      setSelectedUser(null);
      setDeleteOpen(false);
    } catch (error) {
      console.error("Failed to delete user", error);
      // Don't close the dialog on error so user can try again
    }
  };

  const handleEditClick = (user: User) => {
    setSelectedUser(user);
    setForm({ 
      fullName: user.fullName || "", 
      email: user.email || "" 
    });
    setSelectedGroupIds(user.groups ? user.groups.map(g => g.id) : []);
    setEditOpen(true);
  };

  const handleDeleteClick = (user: User) => {
    setSelectedUser(user);
    setDeleteOpen(true);
  };

  if (loading) return <p className="p-8">Loading users...</p>;

  return (
    <div className="min-h-screen bg-white p-8">
      <div className="max-w-7xl mx-auto">
        {/* Header */}
        <div className="mb-8">
          <div className="flex justify-between items-center">
            <div>
              <h1 className="text-3xl font-bold text-gray-900">Users</h1>
              <p className="text-gray-600 text-lg">
                Manage user accounts and group assignments
              </p>
            </div>

            {/* CREATE */}
            <Dialog open={createOpen} onOpenChange={setCreateOpen}>
              <DialogTrigger asChild>
                <Button 
                  className="bg-red-600 hover:bg-red-700 text-white px-6 py-6 text-base font-medium shadow-lg"
                  onClick={() => {
                    setForm({ fullName: "", email: "" });
                    setSelectedGroupIds([]);
                  }}
                >
                  <svg className="w-5 h-5 mr-2" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                    <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M12 4v16m8-8H4" />
                  </svg>
                  Add User
                </Button>
              </DialogTrigger>
              <UserDialog
                open={createOpen}
                onOpenChange={setCreateOpen}
                title="Create User"
                form={form}
                onFormChange={setForm}
                selectedGroupIds={selectedGroupIds}
                onGroupSelectionChange={setSelectedGroupIds}
                availableGroups={availableGroups}
                onSubmit={handleCreate}
                onCancel={() => {
                  setCreateOpen(false);
                  setSelectedGroupIds([]);
                }}
                submitText="Create User"
              />
            </Dialog>
          </div>
        </div>

        {/* Stats */}
        <UserStats users={users} />

        {/* TABLE */}
        <div className="bg-white border border-red-200 rounded-lg shadow-sm overflow-hidden">
          <UserTable 
            users={users}
            onEdit={handleEditClick}
            onDelete={handleDeleteClick}
          />
        </div>

        {/* EDIT DIALOG */}
        <UserDialog
          open={editOpen}
          onOpenChange={setEditOpen}
          title="Edit User"
          form={form}
          onFormChange={setForm}
          selectedGroupIds={selectedGroupIds}
          onGroupSelectionChange={setSelectedGroupIds}
          availableGroups={availableGroups}
          onSubmit={handleEdit}
          onCancel={() => {
            setEditOpen(false);
            setSelectedGroupIds([]);
          }}
          submitText="Save Changes"
        />

        {/* DELETE DIALOG */}
        <Dialog open={deleteOpen} onOpenChange={setDeleteOpen}>
          <DialogContent className="bg-white border-red-200">
            <DialogHeader>
              <DialogTitle className="text-2xl font-bold text-gray-900">Delete User</DialogTitle>
            </DialogHeader>
            <div className="py-4">
              <p className="text-gray-700">
                Are you sure you want to delete the user <strong>{selectedUser?.fullName || selectedUser?.email}</strong>? 
                This action cannot be undone.
              </p>
            </div>
            <DialogFooter>
              <Button 
                variant="outline" 
                onClick={() => setDeleteOpen(false)}
                className="border-gray-300 text-gray-700 hover:bg-gray-50"
              >
                Cancel
              </Button>
              <Button 
                onClick={handleDelete}
                className="bg-red-600 hover:bg-red-700 text-white"
              >
                Delete User
              </Button>
            </DialogFooter>
          </DialogContent>
        </Dialog>
      </div>
    </div>
  );
}
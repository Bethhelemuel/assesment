import { useState, useEffect } from "react";
import { Button } from "@/components/ui/button";
import {
  Dialog,
  DialogContent,
  DialogHeader,
  DialogTitle,
  DialogTrigger,
  DialogFooter,
} from "@/components/ui/dialog";

import { permissionService, type Permission } from "@/service/permissionService";
import { PermissionDialog, PermissionTable, PermissionStats } from "@/components/permissions";

export default function Permissions() {
  const [permissions, setPermissions] = useState<Permission[]>([]);
  const [form, setForm] = useState({ name: "" });
  const [selected, setSelected] = useState<Permission | null>(null);

  const [createOpen, setCreateOpen] = useState(false);
  const [editOpen, setEditOpen] = useState(false);
  const [deleteOpen, setDeleteOpen] = useState(false);

  // Fetch permissions from API
  const fetchPermissions = async () => {
    try {
      const res = await permissionService.getAll();
      setPermissions(res.data);
    } catch (error) {
      console.error("Failed to fetch permissions", error);
    }
  };

  useEffect(() => {
    fetchPermissions();
  }, []);

  // CREATE
  const handleCreate = async () => {
    try {
      console.log("Creating permission:", form.name);
      const res = await permissionService.create(form.name);
      console.log("Create response:", res.data);
      // Refresh data to get the latest from server
      await fetchPermissions();
      setForm({ name: "" });
      setCreateOpen(false);
    } catch (error) {
      console.error("Failed to create permission", error);
    }
  };

  // EDIT
  const handleEdit = async () => {
    if (!selected) return;
    try {
      console.log("Editing permission:", selected.id, "with name:", form.name);
      const res = await permissionService.update(selected.id, form.name);
      console.log("Edit response:", res.data);
      // Refresh data to get the latest from server
      await fetchPermissions();
      setEditOpen(false);
    } catch (error) {
      console.error("Failed to update permission", error);
    }
  };

  // DELETE
  const handleDelete = async () => {
    if (!selected) return;
    try {
      console.log("Deleting permission:", selected.id);
      await permissionService.delete(selected.id);
      // Refresh data to get the latest from server
      await fetchPermissions();
      setSelected(null);
      setDeleteOpen(false);
    } catch (error) {
      console.error("Failed to delete permission", error);
      // Don't close the dialog on error so user can try again
    }
  };

  const handleEditClick = (permission: Permission) => {
    setSelected(permission);
    setForm({ name: permission.name });
    setEditOpen(true);
  };

  const handleDeleteClick = (permission: Permission) => {
    setSelected(permission);
    setDeleteOpen(true);
  };

  return (
    <div className="min-h-screen bg-white p-8">
      <div className="max-w-7xl mx-auto">
        {/* Header */}
        <div className="mb-8">
          <div className="flex justify-between items-center">
            <div>
              <h1 className="text-3xl font-bold text-gray-900">Permissions</h1>
              <p className="text-gray-600 text-lg">
                Manage system permissions and access controls
              </p>
            </div>

            {/* CREATE */}
            <Dialog open={createOpen} onOpenChange={setCreateOpen}>
              <DialogTrigger asChild>
                <Button 
                  className="bg-red-600 hover:bg-red-700 text-white px-6 py-6 text-base font-medium shadow-lg"
                  onClick={() => setForm({ name: "" })}
                >
                  <svg className="w-5 h-5 mr-2" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                    <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M12 4v16m8-8H4" />
                  </svg>
                  Add Permission
                </Button>
              </DialogTrigger>
              <PermissionDialog
                open={createOpen}
                onOpenChange={setCreateOpen}
                title="Create Permission"
                form={form}
                onFormChange={setForm}
                onSubmit={handleCreate}
                onCancel={() => setCreateOpen(false)}
                submitText="Create Permission"
              />
            </Dialog>
          </div>
        </div>

        {/* Stats */}
        <PermissionStats permissions={permissions} />

        {/* TABLE */}
        <div className="bg-white border border-red-200 rounded-lg shadow-sm overflow-hidden">
          <PermissionTable 
            permissions={permissions}
            onEdit={handleEditClick}
            onDelete={handleDeleteClick}
          />
        </div>

        {/* EDIT DIALOG */}
        <PermissionDialog
          open={editOpen}
          onOpenChange={setEditOpen}
          title="Edit Permission"
          form={form}
          onFormChange={setForm}
          onSubmit={handleEdit}
          onCancel={() => setEditOpen(false)}
          submitText="Save Changes"
        />

        {/* DELETE DIALOG */}
        <Dialog open={deleteOpen} onOpenChange={setDeleteOpen}>
          <DialogContent className="bg-white border-red-200">
            <DialogHeader>
              <DialogTitle className="text-2xl font-bold text-gray-900">Delete Permission</DialogTitle>
            </DialogHeader>
            <div className="py-4">
              <p className="text-gray-700">
                Are you sure you want to delete the permission <strong>{selected?.name}</strong>? 
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
                Delete Permission
              </Button>
            </DialogFooter>
          </DialogContent>
        </Dialog>
      </div>
    </div>
  );
}
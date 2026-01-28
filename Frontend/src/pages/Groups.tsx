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

import { groupService, type Group } from "../service/groupService";
import { permissionService } from "@/service/permissionService";
import { GroupDialog, GroupTable, GroupStats } from "@/components/groups";

type Permission = {
  id: number;
  name: string;
};

export default function Groups() {
  const [groups, setGroups] = useState<Group[]>([]);
  const [permissions, setPermissions] = useState<Permission[]>([]);
  const [form, setForm] = useState({ name: "", permissionIds: [] as number[] });
  const [selectedGroup, setSelectedGroup] = useState<Group | null>(null);

  const [createOpen, setCreateOpen] = useState(false);
  const [editOpen, setEditOpen] = useState(false);
  const [deleteOpen, setDeleteOpen] = useState(false);

  // Fetch groups and permissions
  const fetchGroups = async () => {
    try {
      const res = await groupService.getAll();
      setGroups(res.data);
    } catch (error) {
      console.error("Failed to fetch groups", error);
    }
  };

  const fetchPermissions = async () => {
    try {
      const res = await permissionService.getAll();
      setPermissions(res.data);
    } catch (error) {
      console.error("Failed to fetch permissions", error);
    }
  };

  useEffect(() => {
    fetchGroups();
    fetchPermissions();
  }, []);

  // CREATE
  const handleCreate = async () => {
    try {
      console.log("Creating group:", form.name, "with permissions:", form.permissionIds);
      const res = await groupService.create(form.name, form.permissionIds, []);
      console.log("Create response:", res.data);
      // Refresh data to get the latest from server
      await fetchGroups();
      setForm({ name: "", permissionIds: [] });
      setCreateOpen(false);
    } catch (error) {
      console.error("Failed to create group", error);
    }
  };

  // EDIT
  const handleEdit = async () => {
    if (!selectedGroup) return;
    try {
      console.log("Editing group:", selectedGroup.id, "with name:", form.name, "permissions:", form.permissionIds);
      const res = await groupService.update(selectedGroup.id, form.name, form.permissionIds, []);
      console.log("Edit response:", res.data);
      // Refresh data to get the latest from server
      await fetchGroups();
      setEditOpen(false);
    } catch (error) {
      console.error("Failed to update group", error);
    }
  };

  // DELETE
  const handleDelete = async () => {
    if (!selectedGroup) return;
    try {
      console.log("Deleting group:", selectedGroup.id);
      await groupService.delete(selectedGroup.id);
      // Refresh data to get the latest from server
      await fetchGroups();
      setSelectedGroup(null);
      setDeleteOpen(false);
    } catch (error) {
      console.error("Failed to delete group", error);
      // Don't close the dialog on error so user can try again
    }
  };

  const handleEditClick = (group: Group) => {
    setSelectedGroup(group);
    setForm({ 
      name: group.name, 
      permissionIds: group.permissions ? group.permissions.map(p => p.id) : []
    });
    setEditOpen(true);
  };

  const handleDeleteClick = (group: Group) => {
    setSelectedGroup(group);
    setDeleteOpen(true);
  };

  return (
    <div className="min-h-screen bg-white p-8">
      <div className="max-w-7xl mx-auto">
        {/* Header */}
        <div className="mb-8">
          <div className="flex justify-between items-center">
            <div>
              <h1 className="text-3xl font-bold text-gray-900">Groups</h1>
              <p className="text-gray-600 text-lg">
                Manage user groups and their permissions
              </p>
            </div> 

            {/* CREATE */}
            <Dialog open={createOpen} onOpenChange={setCreateOpen}>
              <DialogTrigger asChild>
                <Button 
                  className="bg-red-600 hover:bg-red-700 text-white px-6 py-6 text-base font-medium shadow-lg"
                  onClick={() => setForm({ name: "", permissionIds: [] })}
                >
                  <svg className="w-5 h-5 mr-2" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                    <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M12 4v16m8-8H4" />
                  </svg>
                  Add Group
                </Button>
              </DialogTrigger>
              <GroupDialog
                open={createOpen}
                onOpenChange={setCreateOpen}
                title="Create Group"
                form={form}
                onFormChange={setForm}
                permissions={permissions}
                onSubmit={handleCreate}
                onCancel={() => setCreateOpen(false)}
                submitText="Create Group"
              />
            </Dialog>
          </div>
        </div>

        {/* Stats */}
        <GroupStats groups={groups} />

        {/* TABLE */}
        <div className="bg-white border border-red-200 rounded-lg shadow-sm overflow-hidden">
          <GroupTable 
            groups={groups}
            onEdit={handleEditClick}
            onDelete={handleDeleteClick}
          />
        </div>

        {/* EDIT DIALOG */}
        <GroupDialog
          open={editOpen}
          onOpenChange={setEditOpen}
          title="Edit Group"
          form={form}
          onFormChange={setForm}
          permissions={permissions}
          onSubmit={handleEdit}
          onCancel={() => setEditOpen(false)}
          submitText="Save Changes"
        />

        {/* DELETE DIALOG */}
        <Dialog open={deleteOpen} onOpenChange={setDeleteOpen}>
          <DialogContent className="bg-white border-red-200">
            <DialogHeader>
              <DialogTitle className="text-2xl font-bold text-gray-900">Delete Group</DialogTitle>
            </DialogHeader>
            <div className="py-4">
              <p className="text-gray-700">
                Are you sure you want to delete the group <strong>{selectedGroup?.name}</strong>? 
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
                Delete Group
              </Button>
            </DialogFooter>
          </DialogContent>
        </Dialog>
      </div>
    </div>
  );
}

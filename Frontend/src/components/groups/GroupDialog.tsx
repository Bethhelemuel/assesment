import { Button } from "@/components/ui/button";
import {
  Dialog,
  DialogContent,
  DialogHeader,
  DialogTitle,
  DialogFooter,
} from "@/components/ui/dialog";
import { Input } from "@/components/ui/input";
import { Label } from "@/components/ui/label";
import { PermissionCheckbox } from "./PermissionCheckbox";

interface GroupDialogProps {
  open: boolean;
  onOpenChange: (open: boolean) => void;
  title: string;
  form: { name: string; permissionIds: number[] };
  onFormChange: (form: { name: string; permissionIds: number[] }) => void;
  permissions: Array<{ id: number; name: string }>;
  onSubmit: () => void;
  onCancel: () => void;
  submitText?: string;
}

export const GroupDialog = ({
  open,
  onOpenChange,
  title,
  form,
  onFormChange,
  permissions,
  onSubmit,
  onCancel,
  submitText = "Save"
}: GroupDialogProps) => {
  return (
    <Dialog open={open} onOpenChange={onOpenChange}>
      <DialogContent className="bg-white border-red-200">
        <DialogHeader>
          <DialogTitle className="text-2xl font-bold text-gray-900">{title}</DialogTitle>
        </DialogHeader>
        <div className="space-y-6 py-4">
          <div className="space-y-2">
            <Label className="text-gray-700 font-medium">Group Name</Label>
            <Input
              className="border-gray-300 focus:border-red-500 focus:ring-red-500"
              value={form.name}
              onChange={e => onFormChange({ ...form, name: e.target.value })}
              placeholder="e.g. Admin Group"
            />
          </div>
          
          <PermissionCheckbox
            permissions={permissions}
            selectedIds={form.permissionIds}
            onSelectionChange={(ids) => onFormChange({ ...form, permissionIds: ids })}
          />
        </div>
        <DialogFooter>
          <Button 
            variant="outline" 
            onClick={onCancel}
            className="border-gray-300 text-gray-700 hover:bg-gray-50"
          >
            Cancel
          </Button>
          <Button 
            onClick={onSubmit}
            className="bg-red-600 hover:bg-red-700 text-white"
          >
            {submitText}
          </Button>
        </DialogFooter>
      </DialogContent>
    </Dialog>
  );
};

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
import { GroupCheckbox } from "./GroupCheckbox";

interface Group {
  id: number;
  name: string;
}

interface UserDialogProps {
  open: boolean;
  onOpenChange: (open: boolean) => void;
  title: string;
  form: { fullName: string; email: string };
  onFormChange: (form: { fullName: string; email: string }) => void;
  selectedGroupIds: number[];
  onGroupSelectionChange: (ids: number[]) => void;
  availableGroups: Group[];
  onSubmit: () => void;
  onCancel: () => void;
  submitText?: string;
}

export const UserDialog = ({
  open,
  onOpenChange,
  title,
  form,
  onFormChange,
  selectedGroupIds,
  onGroupSelectionChange,
  availableGroups,
  onSubmit,
  onCancel,
  submitText = "Save"
}: UserDialogProps) => {
  return (
    <Dialog open={open} onOpenChange={onOpenChange}>
      <DialogContent className="bg-white border-red-200">
        <DialogHeader>
          <DialogTitle className="text-2xl font-bold text-gray-900">{title}</DialogTitle>
        </DialogHeader>
        <div className="space-y-6 py-4">
          <div className="space-y-2">
            <Label className="text-gray-700 font-medium">Full Name</Label>
            <Input 
              className="border-gray-300 focus:border-red-500 focus:ring-red-500"
              placeholder="Enter full name"
              value={form.fullName} 
              onChange={e => onFormChange({ ...form, fullName: e.target.value })} 
            />
          </div>
          
          <div className="space-y-2">
            <Label className="text-gray-700 font-medium">Email</Label>
            <Input 
              className="border-gray-300 focus:border-red-500 focus:ring-red-500"
              type="email"
              placeholder="user@example.com" 
              value={form.email} 
              onChange={e => onFormChange({ ...form, email: e.target.value })} 
            />
          </div>
          
          <GroupCheckbox
            groups={availableGroups}
            selectedIds={selectedGroupIds}
            onSelectionChange={onGroupSelectionChange}
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

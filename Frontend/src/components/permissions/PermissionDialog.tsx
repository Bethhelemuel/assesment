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

interface PermissionDialogProps {
  open: boolean;
  onOpenChange: (open: boolean) => void;
  title: string;
  form: { name: string };
  onFormChange: (form: { name: string }) => void;
  onSubmit: () => void;
  onCancel: () => void;
  submitText?: string;
}

export const PermissionDialog = ({
  open,
  onOpenChange,
  title,
  form,
  onFormChange,
  onSubmit,
  onCancel,
  submitText = "Save"
}: PermissionDialogProps) => {
  return (
    <Dialog open={open} onOpenChange={onOpenChange}>
      <DialogContent className="bg-white border-red-200">
        <DialogHeader>
          <DialogTitle className="text-2xl font-bold text-gray-900">{title}</DialogTitle>
        </DialogHeader>
        <div className="space-y-6 py-4">
          <div className="space-y-2">
            <Label className="text-gray-700 font-medium">Permission Name</Label>
            <Input
              className="border-gray-300 focus:border-red-500 focus:ring-red-500"
              value={form.name}
              onChange={e => onFormChange({ name: e.target.value })}
              placeholder="e.g. CanEditUsers"
            />
          </div>
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

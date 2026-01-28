import { Button } from "@/components/ui/button";
import {
  Table,
  TableBody,
  TableCell,
  TableHead,
  TableHeader,
  TableRow,
} from "@/components/ui/table";
import {
  Dialog,
  DialogTrigger,
} from "@/components/ui/dialog";

interface Permission {
  id: number;
  name: string;
}

interface PermissionTableProps {
  permissions: Permission[];
  onEdit: (permission: Permission) => void;
  onDelete: (permission: Permission) => void;
}

export const PermissionTable = ({ permissions, onEdit, onDelete }: PermissionTableProps) => {
  return (
    <Table>
      <TableHeader>
        <TableRow className="bg-gray-50 border-b border-gray-200">
          <TableHead className="font-bold text-gray-900 uppercase text-xs tracking-wider">Permission Name</TableHead>
          <TableHead className="font-bold text-gray-900 uppercase text-xs tracking-wider text-right">Actions</TableHead>
        </TableRow>
      </TableHeader>

      <TableBody>
        {permissions.map(p => (
          <TableRow key={p.id} className="border-b border-gray-200 hover:bg-red-50/50">
            <TableCell className="text-gray-900">
              <div className="flex items-center gap-3">
                <div className="w-10 h-10 rounded-full bg-red-100 flex items-center justify-center">
                  <svg className="w-5 h-5 text-red-600" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                    <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M15 7a2 2 0 012 2m4 0a6 6 0 01-7.743 5.743L11 17H9v2H7v2H4a1 1 0 01-1-1v-2.586a1 1 0 01.293-.707l5.964-5.964A6 6 0 1121 9z" />
                  </svg>
                </div>
                <span className="font-medium">{p.name}</span>
              </div>
            </TableCell>
            <TableCell>
              <div className="flex justify-end gap-2">
                {/* EDIT */}
                <Dialog>
                  <DialogTrigger asChild>
                    <Button
                      size="sm"
                      variant="outline"
                      className="border-gray-300 text-gray-700 hover:bg-gray-50 hover:border-gray-400"
                      onClick={() => onEdit(p)}
                    >
                      <svg className="w-4 h-4 mr-1" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                        <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M11 5H6a2 2 0 00-2 2v11a2 2 0 002 2h11a2 2 0 002-2v-5m-1.414-9.414a2 2 0 112.828 2.828L11.828 15H9v-2.828l8.586-8.586z" />
                      </svg>
                      Edit
                    </Button>
                  </DialogTrigger>
                </Dialog>

                {/* DELETE */}
                <Dialog>
                  <DialogTrigger asChild>
                    <Button 
                      size="sm" 
                      variant="outline"
                      className="border-red-300 text-red-600 hover:bg-red-50 hover:border-red-400"
                      onClick={() => onDelete(p)}
                    >
                      <svg className="w-4 h-4 mr-1" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                        <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M19 7l-.867 12.142A2 2 0 0116.138 21H7.862a2 2 0 01-1.995-1.858L5 7m5 4v6m4-6v6m1-10V4a1 1 0 00-1-1h-4a1 1 0 00-1 1v3M4 7h16" />
                      </svg>
                      Delete
                    </Button>
                  </DialogTrigger>
                </Dialog>
              </div>
            </TableCell>
          </TableRow>
        ))}
      </TableBody>
    </Table>
  );
};

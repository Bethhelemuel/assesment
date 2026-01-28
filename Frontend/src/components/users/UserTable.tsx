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

interface Group {
  id: number;
  name: string;
}

interface User {
  id: number;
  fullName: string;
  email: string;
  groups: Group[];
}

interface UserTableProps {
  users: User[];
  onEdit: (user: User) => void;
  onDelete: (user: User) => void;
}

export const UserTable = ({ users, onEdit, onDelete }: UserTableProps) => {
  const getUserInitials = (fullName?: string) => {
    if (!fullName) return 'U';
    return fullName.split(' ').map(n => n[0]).join('');
  };

  return (
    <Table>
      <TableHeader>
        <TableRow className="bg-gray-50 border-b border-gray-200">
          <TableHead className="font-bold text-gray-900 uppercase text-xs tracking-wider">Full Name</TableHead>
          <TableHead className="font-bold text-gray-900 uppercase text-xs tracking-wider">Email</TableHead>
          <TableHead className="font-bold text-gray-900 uppercase text-xs tracking-wider">Groups</TableHead>
          <TableHead className="font-bold text-gray-900 uppercase text-xs tracking-wider">Actions</TableHead>
        </TableRow>
      </TableHeader>

      <TableBody>
        {users.map((u, index) => (
          <TableRow key={u.id || `user-${index}-${u.email}`} className="border-b border-gray-200 hover:bg-red-50/50">
            <TableCell className="text-gray-900">
              <div className="flex items-center gap-3">
                <div className="w-10 h-10 rounded-full bg-red-100 flex items-center justify-center">
                  <span className="text-red-600 font-semibold text-sm">
                    {getUserInitials(u.fullName)}
                  </span>
                </div>
                <span className="font-medium">{u.fullName || 'Unknown User'}</span>
              </div>
            </TableCell>
            <TableCell className="text-gray-600">{u.email || 'No email'}</TableCell>
            <TableCell>
              {u.groups && u.groups.length > 0 ? (
                <div className="flex gap-2 flex-wrap">
                  {u.groups.map((g) => (
                    <span 
                      key={g.id}
                      className="inline-flex items-center px-3 py-1 rounded-full text-xs font-medium bg-red-100 text-red-800 border border-red-200"
                    >
                      {g.name}
                    </span>
                  ))}
                </div>
              ) : (
                <span className="text-gray-400 italic">No groups assigned</span>
              )}
            </TableCell>
            <TableCell>
              <div className="flex gap-2">
                {/* EDIT */}
                <Dialog>
                  <DialogTrigger asChild>
                    <Button 
                      size="sm" 
                      variant="outline" 
                      className="border-gray-300 text-gray-700 hover:bg-gray-50 hover:border-gray-400"
                      onClick={() => onEdit(u)}
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
                      onClick={() => onDelete(u)}
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

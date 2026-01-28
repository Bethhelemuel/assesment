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

interface UserStatsProps {
  users: User[];
}

export const UserStats = ({ users }: UserStatsProps) => {
  const totalUsers = users.length;
  const withGroups = users.filter(u => u.groups && u.groups.length > 0).length;
  const unassigned = users.filter(u => !u.groups || u.groups.length === 0).length;

  return (
    <div className="grid grid-cols-1 md:grid-cols-3 gap-4 mb-8">
      <div className="bg-white border border-red-200 rounded-lg p-4 shadow-sm">
        <div className="flex items-center justify-between">
          <div>
            <p className="text-sm text-gray-600 uppercase tracking-wider font-medium">Total Users</p>
            <p className="text-3xl font-bold text-gray-900 mt-1">{totalUsers}</p>
          </div>
          <div className="w-12 h-12 bg-red-50 rounded-full flex items-center justify-center">
            <svg className="w-6 h-6 text-red-600" fill="none" stroke="currentColor" viewBox="0 0 24 24">
              <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M12 4.354a4 4 0 110 5.292M15 21H3v-1a6 6 0 0112 0v1zm0 0h6v-1a6 6 0 00-9-5.197M13 7a4 4 0 11-8 0 4 4 0 018 0z" />
            </svg>
          </div>
        </div>
      </div>

      <div className="bg-white border border-red-200 rounded-lg p-4 shadow-sm">
        <div className="flex items-center justify-between">
          <div>
            <p className="text-sm text-gray-600 uppercase tracking-wider font-medium">With Groups</p>
            <p className="text-3xl font-bold text-gray-900 mt-1">{withGroups}</p>
          </div>
          <div className="w-12 h-12 bg-red-50 rounded-full flex items-center justify-center">
            <svg className="w-6 h-6 text-red-600" fill="none" stroke="currentColor" viewBox="0 0 24 24">
              <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M17 20h5v-2a3 3 0 00-5.356-1.857M17 20H7m10 0v-2c0-.656-.126-1.283-.356-1.857M7 20H2v-2a3 3 0 015.356-1.857M7 20v-2c0-.656.126-1.283.356-1.857m0 0a5.002 5.002 0 019.288 0M15 7a3 3 0 11-6 0 3 3 0 016 0zm6 3a2 2 0 11-4 0 2 2 0 014 0zM7 10a2 2 0 11-4 0 2 2 0 014 0z" />
            </svg>
          </div>
        </div>
      </div>

      <div className="bg-white border border-red-200 rounded-lg p-4 shadow-sm">
        <div className="flex items-center justify-between">
          <div>
            <p className="text-sm text-gray-600 uppercase tracking-wider font-medium">Unassigned</p>
            <p className="text-3xl font-bold text-gray-900 mt-1">{unassigned}</p>
          </div>
          <div className="w-12 h-12 bg-red-50 rounded-full flex items-center justify-center">
            <svg className="w-6 h-6 text-red-600" fill="none" stroke="currentColor" viewBox="0 0 24 24">
              <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M12 9v2m0 4h.01m-6.938 4h13.856c1.54 0 2.502-1.667 1.732-3L13.732 4c-.77-1.333-2.694-1.333-3.464 0L3.34 16c-.77 1.333.192 3 1.732 3z" />
            </svg>
          </div>
        </div>
      </div>
    </div>
  );
};

interface Group {
  id: number;
  name: string;
  users: any[];
  permissions: Array<{ id: number; name: string }>;
}

interface GroupStatsProps {
  groups: Group[];
}

export const GroupStats = ({ groups }: GroupStatsProps) => {
  const withPermissions = groups.filter(g => g.permissions && g.permissions.length > 0).length;
  const withoutPermissions = groups.filter(g => !g.permissions || g.permissions.length === 0).length;

  return (
    <div className="grid grid-cols-1 md:grid-cols-2 gap-4 mb-8">
      <div className="bg-white border border-red-200 rounded-lg p-4 shadow-sm">
        <div className="flex items-center justify-between">
          <div>
            <p className="text-sm text-gray-600 uppercase tracking-wider font-medium">With Permissions</p>
            <p className="text-3xl font-bold text-gray-900 mt-1">{withPermissions}</p>
          </div>
          <div className="w-12 h-12 bg-red-50 rounded-full flex items-center justify-center">
            <svg className="w-6 h-6 text-red-600" fill="none" stroke="currentColor" viewBox="0 0 24 24">
              <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M9 12l2 2 4-4m5.618-4.016A11.955 11.955 0 0112 2.944a11.955 11.955 0 01-8.618 3.04A12.02 12.02 0 003 9c0 5.591 3.824 10.29 9 11.622 5.176-1.332 9-6.03 9-11.622 0-1.042-.133-2.052-.382-3.016z" />
            </svg>
          </div>
        </div>
      </div>

      <div className="bg-white border border-red-200 rounded-lg p-4 shadow-sm">
        <div className="flex items-center justify-between">
          <div>
            <p className="text-sm text-gray-600 uppercase tracking-wider font-medium">Without Permissions</p>
            <p className="text-3xl font-bold text-gray-900 mt-1">{withoutPermissions}</p>
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

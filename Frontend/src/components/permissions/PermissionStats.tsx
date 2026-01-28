interface Permission {
  id: number;
  name: string;
}

interface PermissionStatsProps {
  permissions: Permission[];
}

export const PermissionStats = ({ permissions }: PermissionStatsProps) => {
  return (
    <div className="grid grid-cols-1 md:grid-cols-2 gap-4 mb-8">
      <div className="bg-white border border-red-200 rounded-lg p-4 shadow-sm">
        <div className="flex items-center justify-between">
          <div>
            <p className="text-sm text-gray-600 uppercase tracking-wider font-medium">Total Permissions</p>
            <p className="text-3xl font-bold text-gray-900 mt-1">{permissions.length}</p>
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
            <p className="text-sm text-gray-600 uppercase tracking-wider font-medium">Most Common</p>
            <p className="text-xl font-semibold text-gray-900 mt-1">{permissions[0]?.name || "N/A"}</p>
          </div>
          <div className="w-12 h-12 bg-red-50 rounded-full flex items-center justify-center">
            <svg className="w-6 h-6 text-red-600" fill="none" stroke="currentColor" viewBox="0 0 24 24">
              <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M13 7h8m0 0v8m0-8l-8 8-4-4-6 6" />
            </svg>
          </div>
        </div>
      </div>
    </div>
  );
};

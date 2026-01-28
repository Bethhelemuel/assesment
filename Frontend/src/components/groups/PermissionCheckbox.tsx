interface PermissionCheckboxProps {
  permissions: Array<{ id: number; name: string }>;
  selectedIds: number[];
  onSelectionChange: (ids: number[]) => void;
  label?: string;
}

export const PermissionCheckbox = ({ 
  permissions, 
  selectedIds, 
  onSelectionChange, 
  label = "Permissions" 
}: PermissionCheckboxProps) => {
  const handleCheckboxChange = (permissionId: number, checked: boolean) => {
    if (checked) {
      onSelectionChange([...selectedIds, permissionId]);
    } else {
      onSelectionChange(selectedIds.filter(id => id !== permissionId));
    }
  };

  const getSelectedNames = () => {
    return permissions
      .filter(p => selectedIds.includes(p.id))
      .map(p => p.name)
      .join(', ');
  };

  return (
    <div className="space-y-2">
      <label className="text-gray-700 font-medium">{label}</label>
      <div className="border border-gray-300 rounded-md p-3 max-h-32 overflow-y-auto">
        {permissions.length === 0 ? (
          <p className="text-gray-500 text-sm">No permissions available</p>
        ) : (
          <div className="space-y-2">
            {permissions.map(p => (
              <div key={p.id} className="flex items-center space-x-2">
                <input
                  type="checkbox"
                  id={`permission-${p.id}`}
                  checked={selectedIds.includes(p.id)}
                  onChange={(e) => handleCheckboxChange(p.id, e.target.checked)}
                  className="rounded border-gray-300 text-red-600 focus:ring-red-500 focus:border-red-500"
                />
                <label 
                  htmlFor={`permission-${p.id}`}
                  className="text-sm text-gray-700 cursor-pointer hover:text-gray-900"
                >
                  {p.name}
                </label>
              </div>
            ))}
          </div>
        )}
      </div>
      {selectedIds.length > 0 && (
        <div className="mt-2">
          <p className="text-xs text-gray-500">
            Selected: {getSelectedNames()}
          </p>
        </div>
      )}
    </div>
  );
};

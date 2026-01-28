interface Group {
  id: number;
  name: string;
}

interface GroupCheckboxProps {
  groups: Group[];
  selectedIds: number[];
  onSelectionChange: (ids: number[]) => void;
  label?: string;
}

export const GroupCheckbox = ({ 
  groups, 
  selectedIds, 
  onSelectionChange, 
  label = "Groups" 
}: GroupCheckboxProps) => {
  const handleCheckboxChange = (groupId: number, checked: boolean) => {
    if (checked) {
      onSelectionChange([...selectedIds, groupId]);
    } else {
      onSelectionChange(selectedIds.filter(id => id !== groupId));
    }
  };

  const getSelectedNames = () => {
    return groups
      .filter(g => selectedIds.includes(g.id))
      .map(g => g.name)
      .join(', ');
  };

  return (
    <div className="space-y-2">
      <label className="text-gray-700 font-medium">{label}</label>
      <div className="border border-gray-300 rounded-md p-3 max-h-32 overflow-y-auto">
        {groups.length === 0 ? (
          <p className="text-gray-500 text-sm">No groups available</p>
        ) : (
          <div className="space-y-2">
            {groups.map(g => (
              <div key={g.id} className="flex items-center space-x-2">
                <input
                  type="checkbox"
                  id={`group-${g.id}`}
                  checked={selectedIds.includes(g.id)}
                  onChange={(e) => handleCheckboxChange(g.id, e.target.checked)}
                  className="rounded border-gray-300 text-red-600 focus:ring-red-500 focus:border-red-500"
                />
                <label 
                  htmlFor={`group-${g.id}`}
                  className="text-sm text-gray-700 cursor-pointer hover:text-gray-900"
                >
                  {g.name}
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

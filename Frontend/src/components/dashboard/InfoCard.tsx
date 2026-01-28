import { Card, CardContent, CardDescription, CardHeader, CardTitle } from '@/components/ui/card';

interface InfoCardProps {
  title: string;
  value: string;
  subtitle: string;
  icon: React.ReactNode;
  headerIcon: React.ReactNode;
  loading?: boolean;
  valueColor?: string;
  iconBg?: string;
}

export const InfoCard = ({ 
  title, 
  value, 
  subtitle, 
  icon, 
  headerIcon, 
  loading = false,
  valueColor = "text-gray-900",
  iconBg = "bg-red-600"
}: InfoCardProps) => {
  return (
    <Card className="bg-white border-red-200 shadow-sm">
      <CardHeader>
        <div className="flex items-center justify-between">
          <CardTitle className="text-xl text-gray-900">{title}</CardTitle>
          <div className="w-10 h-10 rounded-full bg-red-50 flex items-center justify-center">
            {headerIcon}
          </div>
        </div>
        <CardDescription className="text-gray-600 mt-2">{subtitle}</CardDescription>
      </CardHeader>
      <CardContent>
        <div className="flex items-center gap-4">
          <div className="flex-1">
            <div className={`text-3xl font-bold ${valueColor} mb-1`}>
              {loading ? "..." : value}
            </div>
            <div className="text-sm text-gray-500">{subtitle}</div>
          </div>
          <div className={`w-16 h-16 rounded-xl ${iconBg} flex items-center justify-center shadow-lg`}>
            {icon}
          </div>
        </div>
      </CardContent>
    </Card>
  );
};

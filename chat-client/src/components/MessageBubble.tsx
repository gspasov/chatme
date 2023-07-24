type MessageBubbleProps = {
  isSender: boolean;
  content: string;
  timestamp: number;
};

export default function MessageBubble({
  isSender,
  content,
  timestamp,
}: MessageBubbleProps) {
  function formatDate(date: Date): string {
    const dayOptions: Intl.DateTimeFormatOptions = { weekday: "short" };
    const timeOptions: Intl.DateTimeFormatOptions = {
      hour: "2-digit",
      minute: "2-digit",
    };

    const dayName = new Intl.DateTimeFormat("en-US", dayOptions).format(date);
    const timeString = date.toLocaleTimeString("en-US", timeOptions);

    return `${dayName} at ${timeString}`;
  }

  return (
    <div className={`flex mb-2 ${isSender ? "justify-end" : "justify-start"}`}>
      <div
        className={`rounded-2xl px-4 py-2 ${
          isSender ? "bg-blue-500 text-white" : "bg-gray-300"
        }`}
      >
        <span>{content}</span>
        <div
          className={
            isSender ? "text-xs text-gray-300" : "text-xs text-gray-600"
          }
        >
          {formatDate(new Date(timestamp))}
        </div>
      </div>
    </div>
  );
}

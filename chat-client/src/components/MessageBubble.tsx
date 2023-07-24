type MessageBubbleProps = {
  isSender: boolean;
  content: string;
};

export default function MessageBubble({
  isSender,
  content,
}: MessageBubbleProps) {
  return (
    <div className={`flex mb-2 ${isSender ? "justify-end" : "justify-start"}`}>
      <div
        className={`rounded-lg px-4 py-2 ${
          isSender ? "bg-blue-500 text-white" : "bg-gray-300"
        }`}
      >
        {content}
      </div>
    </div>
  );
}

export function buildConversationId(id1: string, id2: string): string {
  return [id1, id2].sort().join("#");
}

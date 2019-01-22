local Socket        = require("omg.net.tcp")

local Packet = {
-- client -> server
  CS_LOGIN              = 0x0000,
  CS_CHAT               = 0x0001,

-- server -> client
  SC_DIALOG             = 0x0000,
  SC_CHAT               = 0x0001,

  SC_ADD_USER           = 0x0100,
  SC_REMOVE_USER        = 0x0101,
}

-- client -> server
Packet[Packet.CS_LOGIN] = function (remote, data)
  local user = data:read_string()
  if not data:completed() then return true end

  remote:recv_login(user)
  return false
end

Packet[Packet.CS_CHAT] = function (remote, data)
  local message = data:read_string()
  local playerSrc = data:read_string()
  local playerDes = data:read_string()

  if not data:completed() then return true end

  remote:recv_chat(message, playerSrc, playerDes)
  return false
end

-- server -> client
Packet.make_dialog = function (message)
  local w = Socket.packet(Packet.SC_DIALOG)
  w:append_string(message)
  w:finish()
  return w
end

Packet.make_chat = function (user, message, talkPlayer)
  local w = Socket.packet(Packet.SC_CHAT)
  w:append_string(user)
  w:append_string(message)
  w:append_string(talkPlayer)
  w:finish()
  return w
end

Packet.make_add_user = function (uid, user, selfUid)
  local w = Socket.packet(Packet.SC_ADD_USER)
  w:append_uint16(uid)
  w:append_string(user)
  w:append_uint16(selfUid)
  w:finish()
  return w
end

Packet.make_remove_user = function (uid)
  local w = Socket.packet(Packet.SC_REMOVE_USER)
  w:append_uint16(uid)
  w:finish()
  return w
end

return Packet

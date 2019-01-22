local Packet      = require("chat.packet")
local Socket      = require("omg.net.tcp")
local Remote      = require("chat.remote")

local function onerror(socket, errcode, errstr)
  print("ERROR:", socket:getsockname(), errcode, errstr)
end

local function onconnected(socket, listen)
  socket.remote = Remote(socket)
end

local function ondisconnected(socket)
  socket.remote:disconnected()
  socket.remote = nil;
end

local function onrecv(socket, data)
  local packet_id = data:get_id()
  local func = Packet[packet_id]
  if func == nil then
    print("ERROR: packet_id not found", packet_id)
    socket:close()
  end

  if func(socket.remote, data) then
    print("ERROR: packet_id invalid data", packet_id)
    socket:close()
  end
end

local function network(config)
  local listener = Socket.packet_server({
    host            = config.host,
    port            = config.port,
    onerror         = onerror,
    onconnected     = onconnected,
    ondisconnected  = ondisconnected,
    onrecv          = onrecv,
    oneof           = oneof,
  })

  return listener
end

return network

variable "rg_name" {
  description = "The name of the resource group to deploy to"
}

variable "location" {
  description = "The location for the resources"
}

variable "sb_name" {
  description = "The name of the service bus resource"
}

variable "sb_topic_name" {
  description = "The name of the topic resource"
}

variable "sb_subscription_name" {
  description = "The name of the topic subscription resource"
}

resource "azurerm_resource_group" "rg" {
  name     = "${var.rg_name}"
  location = "${var.location}"
}

resource "azurerm_servicebus_namespace" "namespace" {
  name                = "${var.sb_name}"
  location            = "${var.location}"
  resource_group_name = "${azurerm_resource_group.rg.name}"
  sku                 = "Standard"
}

resource "azurerm_servicebus_topic" "topic" {
  name                = "${var.sb_topic_name}"
  resource_group_name = "${azurerm_resource_group.rg.name}"
  namespace_name      = "${azurerm_servicebus_namespace.namespace.name}"

  enable_partitioning = true
  support_ordering    = true
}

resource "azurerm_servicebus_subscription" "subscription" {
  name                = "${var.sb_subscription_name}"
  resource_group_name = "${azurerm_resource_group.rg.name}"
  namespace_name      = "${azurerm_servicebus_namespace.namespace.name}"
  topic_name          = "${azurerm_servicebus_topic.topic.name}"
  requires_session    = true
  max_delivery_count  = 5
}

output "connection_string" {
  value = "${azurerm_servicebus_namespace.namespace.default_primary_connection_string}"
}
